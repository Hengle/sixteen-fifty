using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SixteenFifty.EventItems {
  using Commands;

  /**
   * \brief
   * Describes a dialogue in which all speakers appear at once,
   * speaking is unclear, and then everyone disappears.
   *
   * In a simple dialogue, speakers don't appear and disappear during
   * the text.
   * Speakers appear at the very beginning, some text is played out,
   * and the speakers disappear.
   */
  [CreateAssetMenu(menuName = "1650/Simple Dialogue")]
  public class SimpleDialogue : BasicScriptedEvent, IEquatable<SimpleDialogue> {
    public SpeakerConfiguration[] speakerConfigurations;
    public string[] messages;

    public override IScript Compile() =>
      new SimpleDialogueScript(speakerConfigurations, messages);

    public bool Equals(SimpleDialogue that) =>
      that != null &&
      this.speakerConfigurations.SequenceEqual(that.speakerConfigurations) &&
      this.messages.SequenceEqual(that.messages);
  }

  [SelectableSubtype(friendlyName = "Simple dialogue (inline)")]
  public class SimpleDialogueScript : IScript, IEquatable<SimpleDialogueScript> {
    public SpeakerConfiguration[] speakerConfigurations;
    public string[] messages;

    public SimpleDialogueScript(
      SpeakerConfiguration[] speakerConfigurations,
      string[] messages) {
      this.speakerConfigurations = speakerConfigurations;
      this.messages = messages;
    }

    public bool Equals(SimpleDialogueScript that) =>
      that != null &&
      this.speakerConfigurations.SequenceEqual(that.speakerConfigurations) &&
      this.messages.SequenceEqual(that.messages);

    public bool Equals(IScript that) =>
      IEquatableExt.Equals(this, that);
    
    public Command<object> GetScript(EventRunner runner) {
      var manager = runner.Manager;

      var t = manager.GetComponent<RectTransform>();
      var speakers = new List<Speaker>();
      return Command<object>.Action(() => manager.BlocksRaycasts = true)
        .ThenPure(
          _ =>
          speakers = speakerConfigurations.Select(
            s =>
            Speaker.Construct(
              manager.speakerPrefab,
              s.speakerData,
              s.position,
              t,
              s.orientation)
            .WithAlpha(0))
          .ToList())
        .Then(
          ss => ss.Traverse_(s => Lerp.FadeSpriteRenderer(s.GetComponent<SpriteRenderer>(), FadeDirection.IN)))
        .Then(_ => Lerp.FadeImage(runner.Manager.dialogueTextBox, FadeDirection.IN))
        .Then(_ => new Paragraph(runner, messages))
        .Then(_ => Lerp.FadeImage(runner.Manager.dialogueTextBox, FadeDirection.OUT))
        .Then(
          _ => speakers.Traverse_(
            s => Lerp.FadeSpriteRenderer(s.GetComponent<SpriteRenderer>(), FadeDirection.OUT)))
        .ThenAction(
          _ => {
            foreach(var s in speakers) {
              GameObject.Destroy(s.gameObject);
            }
          })
        .ThenAction(_ => manager.BlocksRaycasts = false);
    }
  }
}
