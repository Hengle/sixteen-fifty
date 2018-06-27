using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty {
  namespace Commands {
    /**
    * A paragraph is a compound command, built by sequencing ShowText
    * and AwaitClick commands.
    */
    public class Paragraph : Command<object> {
      private Command<object> inner;

      public Paragraph(EventRunner runner, string[] messages) {
        inner = CompileCommand(runner, messages);
      }

      private Command<object> CompileCommand(EventRunner runner, string[] messages) {
        var cmd = Command<object>.Empty;
        foreach(var message in messages) {
          cmd = cmd
            .Then(_ => new ShowText(runner, message))
            .Then(_ => new AwaitClick(runner));
        }
        return cmd.Then(_ => ShowText.Clear(runner));
      }

      public override IEnumerator GetCoroutine() {
        return inner.GetCoroutine();
      }
    }
  }
}
