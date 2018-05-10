using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands {
  /**
   * \brief
   * A wrapper for a coroutine computing a result of type `T`.
   *
   * - Commands are the basis for events: a scripted event is a
   *   sequence of commands.
   * - Commands can be chained using the `Then` method, which takes a
   *   function from the result of the current command to compute a
   *   new command to execute.
   * - This structure allows event scripts to be programmed in a way
   *   resembling promises.
   */
  public abstract class Command<T> {
    protected T result;
    /**
     * \brief
     * The result of the command.
     * 
     * In general, you should never read this property directly.
     * It's best to obtain the result of a command by chaining using
     * BindExt::Then.
     */
    public T Result => result;

    /**
     * \brief
     * The coroutine that implements the command.
     */
    public abstract IEnumerator GetCoroutine();
  
    /**
     * \brief
     * Construct a one-off command using the given function to enumerate the frames.
     *
     * Usually, this isn't super useful because C# doesn't allow using
     * the `yield return` syntax inside an anonymous function.
     */
    public static Command<T> Invent(Func<IEnumerator> invention) {
      return new InventedCommand<T>(invention);
    }

    /**
     * \brief 
     * Constructs a command that executes the given action, yielding a null result.
     *
     * \sa
     * ActionCommand
     * #ThenAction
     * #Pure
     * PureExt
     */
    public static Command<object> Action(Action action) => new ActionCommand(action);

    /**
     * \brief
     * Constructs a command that executes the given function and yields its result.
     *
     * \sa
     * PureExt
     */
    public static Command<T> Pure(Func<T> f) => new PureCommand<T>(f);

    /**
     * \brief
     * Constructs a command that does nothing and yields a null result.
     */
    public static Command<object> Empty => Action(() => { return; });

    /**
     * \brief
     * Chains this command with an Action.
     *
     * \sa
     * ActionCommand
     * PureExt
     * BindExt
     */
    public Command<object> ThenAction(Action<T> continuation) {
      return this.Then(t => Command<object>.Action(() => continuation(t)));
    }
  }

  /**
   * \brief
   * An invented command is a command that just executes a given function as its IEnumerator.
   *
   * \sa
   * Command::Invent
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

  /**
   * \brief
   * A Command that invokes a given Action to create effects and has a
   * null result.
   *
   * \sa
   * Command::ThenAction
   */
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

  /**
   * \brief
   * A Command that invokes a given function to compute its result.
   */
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
   * \brief
   * Runs one Command, and feeds its result into a Func that
   * computes the next Command to run.
   *
   * This is a composite Command that runs the given Command, and then
   * feeds the result of that Command into the provided Func to
   * compute a new Command to run.
   *
   * Essentially, this is a _monadic bind operation_.
   *
   * \sa
   * BindExt::Then
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
   * \brief
   * A compound command that executes two coroutines at once, by interleaving them.
   *
   * This command ends when either of its subcommands ends, returning
   * the result in an Either.
   *
   * \sa
   * RaceExt::And
   *
   * \warning
   * Using Race is somewhat dangerous because it completely
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
     * \brief
     * Actually, this is fmap.
     */
    public static Command<R> ThenPure<T, R>(this Command<T> self, Func<T, R> k) {
      return self.Then(t => Command<R>.Pure(() => k(t)));
    }
  }
  
  public static class BindExt {
    /**
     * \brief
     * Sequences a command after this one by passing the result of
     * this command to a given function that computes a new command.
     */
    public static Command<R> Then<T, R>(this Command<T> self, Func<T, Command<R>> k) {
      return new BindCommand<T, R>(self, k);
    }
  }

  /**
   * \brief
   * Extension methods for transforming an IEnumerable into a Command.
   */
  public static class TraverseExt {
    /**
     * \brief
     * Computes a command for each element of an IEnumerable and
     * sequences all the commands.
     *
     * The results of the commands are collected into a List that
     * becomes the result of this command.
     *
     * \sa
     * SequenceExt::Sequence
     */
    public static Command<List<R>> Traverse<T, R>(this IEnumerable<T> self, Func<T, Command<R>> f) {
      return self.Select(x => f(x)).Sequence();
    }

    /**
     * \brief
     * Computes a unit command for each element of an IEnumerable and sequences all the commands.
     *
     * The results of the commands are thrown away. (They're all null anyway.)
     *
     * \sa
     * SequenceExt::Sequence_
     */
    public static Command<object> Traverse_<T>(this IEnumerable<T> self, Func<T, Command<object>> f) {
      return self.Select(x => f(x)).Sequence_();
    }
  }

  /**
   * \brief
   * Extension methods for collapsing an IEnumerable<Command> into a single Command.
   */
  public static class SequenceExt {
    /**
     * \brief
     * Compiles a sequence of commands into a single command that
     * executes the sequence.
     *
     * The results of the commands are collected into a list that is
     * yielded as the result of this command.
     *
     * \sa
     * TraverseExt::Traverse
     */
    public static Command<List<T>> Sequence<T>(this IEnumerable<Command<T>> self) {
      var list = new List<T>();
      var acc = Command<object>.Empty;
      foreach(var cmd in self) {
        acc = acc.Then(_ => cmd).ThenAction(r => list.Add(r));
      }
      return acc.ThenPure(_ => list);
    }

    /**
     * \brief
     * Compiles a sequence of commands into a single command that
     * executes the sequence.
     *
     * The results of the commands are thrown away. (They're null anyway.)
     *
     * \sa
     * TraverseExt::Traverse_
     */
    public static Command<object> Sequence_(this IEnumerable<Command<object>> self) {
      var acc = Command<object>.Empty;
      foreach(var cmd in self) {
        acc = acc.Then(_ => cmd);
      }
      return acc;
    }
  }
  
  /**
   * \brief
   * Extension methods for performing multiple Commands concurrently.
   */
  public static class RaceExt {
    /**
     * \brief
     * Runs the given Command concurrently with this once.
     */
    public static Command<Either<T, S>> And<T, S>(this Command<T> self, Command<S> that) {
      return new Race<T, S>(self, that);
    }
  }
}
