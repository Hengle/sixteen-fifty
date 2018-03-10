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
  
    public Command<R> Then<R>(Func<T, Command<R>> continuation) {
      return new BindCommand<T, R>(this, continuation);
    }
  
    public Command<Either<T, S>> And<S>(Command<S> that) {
      return new Race<T, S>(this, that);
    }
  
    public static Command<T> Invent(Func<IEnumerator> invention) {
      return new InventedCommand<T>(invention);
    }

    public static Command<T> Action(Action action) {
      return new ActionCommand<T>(action);
    }

    public static Command<T> Empty => new ActionCommand<T>(() => { return; });
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

  class ActionCommand<T> : Command<T> {
    private Action action;
    public ActionCommand(Action action) {
      this.action = action;
    }
    public override IEnumerator GetCoroutine() {
      action();
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
