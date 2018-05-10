using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Commands;

/**
 * \brief
 * Controls an InteractionMenu.
 *
 * This IScript is used to manage an InteractionMenu, including fading
 * it in and out, waiting for the user to click on an option or to
 * dismiss the menu, and running the EventScript associated with the
 * chosen Interaction.
 */
public class ControlInteractionMenu : IScript {
  IEnumerable<Interaction> interactions;
  PlayerController player;

  public ControlInteractionMenu(PlayerController player, IEnumerable<Interaction> interactions) {
    this.interactions = interactions;
    this.player = player;
  }

  public Command<object> GetScript(EventRunner runner) {
    var manager = runner.Manager;
    Debug.Assert(null != manager, "event manager is not null");
    var menu = manager.interactionMenu;
    Debug.Assert(null != menu, "interaction menu is not null");

    Either<Interaction, PointerEventData> ev = null;
    
    return
      Command<object>.Action(() => manager.BlocksRaycasts = true)
      .Then(
        _ => {
          var target = player.transform.position;
          target.z = Camera.main.transform.position.z;
          return new MoveTransform(Camera.main.transform, target);
        })
      .ThenAction(_ => menu.transform.position = player.transform.position - new Vector3(4f, 0f, 0f))
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
      .ThenAction(_ => menu.MenuActive = manager.BlocksRaycasts = false)
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
