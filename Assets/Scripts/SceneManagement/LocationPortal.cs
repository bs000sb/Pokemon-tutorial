using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    // Teleports player to different position without switching scenes

public class LocationPortal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;


    PlayerController player;
    public void OnPlayerTriggered(PlayerController player)
    {
        player.Character.Animator.IsMoving = false;
        this.player = player;
        StartCoroutine(Teleport());
    }

    Fader fader;

    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }

    IEnumerator Teleport()
    {
        GameController.Instance.PausedGame(true);
        yield return fader.FadeIn(0.5f);

        var desPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Character.SetPositionandSnapToTile(desPortal.SpawnPoint.position);

        yield return fader.FadeOut(0.5f);
        GameController.Instance.PausedGame(false);
    }

    public Transform SpawnPoint => spawnPoint;
}

public enum DestinationIdentifier { A, B, C, D, E }

