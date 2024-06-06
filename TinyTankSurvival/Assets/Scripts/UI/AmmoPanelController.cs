using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject missileIconPrefab;

    private GameUpgrades gameUpgrades;
    private GameHandler gHandler;
    private List<MissileIconController> ammoIconScripts;
    private AmmoManager ammoManager;
    private PlayerShooting playerShooting;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        ammoIconScripts = new List<MissileIconController>();
        gameUpgrades = GameUpgrades.Instance;
        gHandler = GameHandler.Instance;
        StartCoroutine(WaitForPlayerCharacter());
    }

    private IEnumerator WaitForPlayerCharacter()
    {
        while (gHandler.PlayerCharacter == null)
        {
            yield return null;
        }
        var character = gHandler.PlayerCharacter;

        // Setting up initial ammo icons
        ammoManager = character.GetComponent<AmmoManager>();
        playerShooting = character.GetComponent<PlayerShooting>();

        // As a race-condition precaution, waiting until there's ammo
        while (ammoManager.AmmoCapacity <= 0)
        {
            yield return null;
        }

        for (int i = 0; i < ammoManager.AmmoCapacity; i++)
        {
            var icon = Instantiate(missileIconPrefab, transform);
            var iconScript = icon.GetComponent<MissileIconController>();
            ammoIconScripts.Add(iconScript);
            iconScript.Setup(ammoManager);
            if (i < ammoManager.Ammo)
            {
                // Ammo slot filled
                iconScript.UpdateProgress(1);
            } else
            {
                // Ammo slot not filled
                iconScript.UpdateProgress(0);
            }
            
        }

        // Listening to reloads and shots
        playerShooting.OnPlayerShooting += PlayerShooting;
        ammoManager.OnAmmoReloadProgress += AmmoReloading;
        ammoManager.OnAmmoReloaded += AmmoReloaded;

        // Dumb unity bug work around
        GetComponent<HorizontalLayoutGroup>().enabled = false;
        startTime = Time.time;
        StartCoroutine(RefreshHorLayout());
    }

    /// <summary>
    /// Unity bug? The missile icons arent spaced out as they should :/ Waits for 1 second 
    /// </summary>
    private IEnumerator RefreshHorLayout()
    {
        while (Time.time < startTime + 1.0f)
        {
            yield return null;
        }

        GetComponent<HorizontalLayoutGroup>().enabled = true;
    }

    private void PlayerShooting()
    {
        // Setting all ammo above current ammo (after shot) as empty
        for (int i = ammoManager.Ammo; i < ammoManager.AmmoCapacity; i++)
        {
            ammoIconScripts[i].UpdateProgress(0);
        }
    }

    private void AmmoReloading(float progress)
    {
        if (progress >= 1)
            return;

        ammoIconScripts[ammoManager.Ammo].UpdateProgress(progress);
    }

    private void AmmoReloaded(int currentAmmo)
    {
        ammoIconScripts[currentAmmo-1].UpdateProgress(1);
    }
}
