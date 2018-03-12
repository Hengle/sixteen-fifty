using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands {
  /**
   * An abstract command producing a result of type T.
   * Commands are the basis for events: a scripted event is a sequence
   * of commands.
   * Commands can be chained using the `Then` method, which takes a
   * function from the result of the current command to compute a new
   * command to execute.
   * This structure allows event scripts to be programmed in a way
   * resembling promises.
   */
  public abstract class Command<T> {
    protected T result;
    public T Result => result;
    public abstract IEnumerator GetCoroutine();
  
    public virtual Command<R> Then<R>(Func<T, Command<R>> continuation) {
      return new BindCommand<T, R>(this, continuation);
    }

    public virtual Command<object> Traverse_(Func<T, IEnumerable<Command<object>>> continuation) {
      return new BindCommand<T, object>(
        this,
        t => {
          var acc = Command<object>.Empty;
          foreach(var cmd in continuation(t)) {
            acc = acc.Then(_ => cmd);
          }
          return acc;
        });
    }

    public virtual Command<List<R>> Traverse<R>(Func<T, IEnumerable<Command<R>>> continuation) {
      return new BindCommand<T, List<R>>(
        this,
        t => {
          var list = new List<R>();
          var acc = Command<object>.Empty;
          // idea: compute each command and stick its result in the list
          foreach(var cmd in continuation(t)) {
            acc = acc.Then(_ => cmd).ThenAction(r => list.Add(r));
          }
          // finally, yield the list.
          return acc.ThenPure(_ => list);
        });
    }

    /**
     * Actually, this is fmap.
     */
    public Command<R> ThenPure<R>(Func<T, R> continuation) {
      return Then(t => Command<R>.Pure(() => continuation(t)));
    }

    public Command<object> ThenAction(Action<T> continuation) {
      return Then(t => Command<object>.Action(() => continuation(t)));
    }
  
    public Command<Either<T, S>> And<S>(Command<S> that) {
      return new Race<T, S>(this, that);
    }
  
    public static Command<T> Invent(Func<IEnumerator> invention) {
      return new InventedCommand<T>(invention);
    }

    public static Command<object> Action(Action action) => new ActionCommand(action);

    public static Command<T> Pure(Func<T> f) => new PureCommand<T>(f);

    public static Command<object> Empty => new ActionCommand(() => { return; });
  }
  
  /**
   * An invented command is a command that just executes a given function.
   */
  class InventedCommand<T> : Command<T> {
    private Func<IEnumerator> invention;
    public InventedCommand(Func<IEnumerator> invention) {
      this.invention = invention;
    }
  
    public override IEnumerator GetCoroutine() {
      return invention();
    }
  }

  class ActionCommand : Command<object> {
    private Action action;
    public ActionCommand(Action action) {
      this.action = action;
    }

    public override IEnumerator GetCoroutine() {
      action();
      result = null;
      yield break;
    }
  }

  class PureCommand<T> : Command<T> {
    private Func<T> f;
    public PureCommand(Func<T> f) {
      this.f = f;
    }

    public override IEnumerator GetCoroutine() {
      result = f();
      yield break;
    }
  }
  
  /**
   * This is a composite command that runs the given command, and then
   * feeds the result of that command into the provided continuation to
   * compute a new command that is executed.
   * Essentially, this is a monadic bind operation.
   * This class is used internally to implement the `Then` method of Command.
   */
  class BindCommand<S, T> : Command<T> {
    private Func<S, Command<T>> continuation;
    private Command<S> command;
  
    public BindCommand(Command<S> cmd, Func<S, Command<T>> continuation) {
      command = cmd;
      this.continuation = continuation;
    }
  
    public override IEnumerator GetCoroutine() {
      foreach(var obj in new TrivialEnumerable(command.GetCoroutine())) {
        yield return obj;
      }
      var next = continuation(command.Result);
      foreach(var obj in new TrivialEnumerable(next.GetCoroutine())) {
        yield return obj;
      }
      result = next.Result;
    }
  }
  
  /**
   * A compound command that executes two coroutines at once, by interleaving them.
   * This command ends when either of its subcommands ends, returning
   * the result in an Either.
   * WARNING: using Race is somewhat dangerous because it completely
   * ignores what the underlying coroutines being raced return.
   * This means that returning things such as WaitForSeconds from the
   * component coroutines will not work!
   */
  class Race<S, T> : Command<Either<S, T>> {
    private Command<S> sCmd;
    private Command<T> tCmd;
  
    public Race(Command<S> cmd1, Command<T> cmd2) {
      sCmd = cmd1;
      tCmd = cmd2;
    }
  
    public override IEnumerator GetCoroutine() {
      var coro1 = sCmd.GetCoroutine();
      var coro2 = tCmd.GetCoroutine();
  
      while(true) {
        if(!coro1.MoveNext()) {
          result = new Either<S, T>(sCmd.Result);
          yield break;
        }
  
        if(!coro2.MoveNext()) {
          result = new Either<S, T>(tCmd.Result);
          yield break;
        }
  
        yield return null;
      }
    }
  }
}
