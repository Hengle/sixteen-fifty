using System;
using System.Linq;
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
  
    /**
     * Construct a one-off command using the given function to enumerate the frames.
     */
    public static Command<T> Invent(Func<IEnumerator> invention) {
      return new InventedCommand<T>(invention);
    }

    /**
     * Returns a command that executes the given action, yielding a null result.
     */
    public static Command<object> Action(Action action) => new ActionCommand(action);

    /**
     * Returns a command that executes the given function and yields its result.
     */
    public static Command<T> Pure(Func<T> f) => new PureCommand<T>(f);

    /**
     * Returns a command with no effects and a null result object.
     */
    public static Command<object> Empty => Action(() => { return; });

    public Command<object> ThenAction(Action<T> continuation) {
      return this.Then(t => Command<object>.Action(() => continuation(t)));
    }
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

  public static class PureExt {
    /**
     * Actually, this is fmap.
     */
    public static Command<R> ThenPure<T, R>(this Command<T> self, Func<T, R> k) {
      return self.Then(t => Command<R>.Pure(() => k(t)));
    }
  }
  
  public static class BindExt {
    /**
     * Sequences a command to occur after this command.
     * The next command can access the result of the current command,
     * and use this result to determine which next command should be
     * executed.
     */
    public static Command<R> Then<T, R>(this Command<T> self, Func<T, Command<R>> k) {
      return new BindCommand<T, R>(self, k);
    }
  }

  public static class TraverseExt {
    public static Command<List<R>> Traverse<T, R>(this IEnumerable<T> self, Func<T, Command<R>> f) {
      return self.Select(x => f(x)).Sequence();
    }

    public static Command<object> Traverse_<T>(this IEnumerable<T> self, Func<T, Command<object>> f) {
      return self.Select(x => f(x)).Sequence_();
    }
  }

  public static class SequenceExt {
    /**
     * Compiles a sequence of commands into a single command that
     * executes the sequence.
     */
    public static Command<List<T>> Sequence<T>(this IEnumerable<Command<T>> self) {
      var list = new List<T>();
      var acc = Command<object>.Empty;
      foreach(var cmd in self) {
        acc = acc.Then(_ => cmd).ThenAction(r => list.Add(r));
      }
      return acc.ThenPure(_ => list);
    }

    public static Command<object> Sequence_(this IEnumerable<Command<object>> self) {
      var acc = Command<object>.Empty;
      foreach(var cmd in self) {
        acc = acc.Then(_ => cmd);
      }
      return acc;
    }
  }
  
  public static class RaceExt {
    public static Command<Either<T, S>> And<T, S>(this Command<T> self, Command<S> that) {
      return new Race<T, S>(self, that);
    }
  }
}
