using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Commands;

public class ControlInteractionMenu : IScript {
  IEnumerable<Interaction> interactions;

  public ControlInteractionMenu(IEnumerable<Interaction> interactions) {
    this.interactions = interactions;
  }

  public Command<object> GetScript(EventRunner runner) {
    var manager = runner.Manager;
    var menu = manager.interactionMenu;

    Either<Interaction, PointerEventData> ev = null;
    
    return
      Command<object>.Action(() => manager.BlocksRaycasts = true)
      .ThenAction(_ => menu.CreateMenu(interactions))
      .ThenAction(_ => menu.MenuActive = true)
      .Then(_ => new FadeInteractionMenu(menu, FadeDirection.IN))
      .Then(
        _ => new Race<Interaction, PointerEventData>(
          // either we click on a button in the menu
          new AwaitAction<Interaction>(
            a => { menu.Interacted += a; },
            a => { menu.Interacted -= a; }),
          // or we click outside the menu
          new AwaitAction<PointerEventData>(
            a => { manager.MainPanelClicked += a; },
            a => { manager.MainPanelClicked -= a; })))
      .ThenAction(e => ev = e)
      .Then(_ => new FadeInteractionMenu(menu, FadeDirection.OUT))
      .ThenAction(_ => menu.DestroyMenu())
      .ThenAction(_ => manager.BlocksRaycasts = false)
      .ThenAction(_ => menu.MenuActive = false)
      .Then(
        _ => ev.Eliminate(
          // if we clicked on a button, then we run its associated event script
          interaction => {
            Debug.Log("Clicked on a button!");
            return interaction.script.GetScript(runner);
          },
          // if we clicked outside, then there's nothing to do.
          click => {
            Debug.Log("Clicked outside the menu!");
            return Command<object>.Empty;
          }));
  }
}
