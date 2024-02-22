using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AquaCulture;

public class Pressure : MonoBehaviour {
    public int pressure;
    float gaugeMultiplier;
    public float[] gaugeMultiplierForEachUpgrade = new float[5];
    public float lowestSafeDepth = 1.0f;
    public float dialRotation;
    float timeSinceLastPrint;
    public GameObject player;
    public GameObject pressureNeedle;
    public float greenThreshold;
    public float redThreshold;
    public Image pressureColourRingImage;
    public GameObject pressureColourRing;
    bool damagerRunning = false;
    public Damageable playerDamagable;

    protected InventoryController PlayerInventory;
    // Use this for initialization
    void Start () {
        pressureColourRing.SetActive(true);
        playerDamagable = PlayerCharacter.PlayerInstance.GetComponent<Damageable>();
        PlayerInventory = PlayerCharacter.PlayerInstance.GetComponent<InventoryController>();
        gaugeMultiplier = gaugeMultiplierForEachUpgrade[PlayerInventory.GetNumberOfItem("pressureUpgrade")];

    }

    // Update is called once per frame
    void Update()
    {

        //timeSinceLastPrint = timeSinceLastPrint + Time.deltaTime;
        timeSinceLastPrint += Time.deltaTime;

        //if (timeSinceLastPrint > 2.0f)
        //{
        //    printPressure();
        //    timeSinceLastPrint = 0.0f;
        //}

        dialRotation = -140.0f;
        // Note: Position gets more positive the higher up the screen we go, while depth goes more negative. So when comparing them we should take the negative of the position.
        // If Ellen is above lowestSafeDepth then set the needle as far left as it can go (-140 degrees)
        if (player.transform.position.y > -lowestSafeDepth)
        {
            dialRotation += 0.0f;
        }
        else
        {
            // Ellen is below lowestSafeDepth
            // The rotation will be -140 degrees plus a number which gets bigger the deeper we go.
            //  -player.transform.position.y -> the depth is minus the position
            //  -player.transform.position.y - lowestPressureDepth -> starts counting up from lowestPressureDepth
            // gaugeMultiplier -> this is how much the difference between the player depth and the lowest safe depth affects the degrees round the needle is going.
            dialRotation += gaugeMultiplier * (-player.transform.position.y - lowestSafeDepth);
        }

        // The dial cannot go more than plus or minus 140 degrees in any direction.
        dialRotation = Mathf.Clamp(dialRotation, -140.0f, 140.0f);

        pressureNeedle.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -dialRotation));

        setColour();

        if ((dialRotation > redThreshold) && (dialRotation < redThreshold + 5))
        {
            StartCoroutine(Blink(0.3f));
        }

        if ((dialRotation > redThreshold) && (damagerRunning == false))
        {
            StartCoroutine(DamagePlayer(5.0f));
            damagerRunning = true;
        }

        if ((dialRotation < redThreshold) && (damagerRunning == true)) { damagerRunning = false; }
    }

    void printPressure()
    {
        if (player.transform.position.y < -5.0f)
        {
            Debug.Log("ALERT PRESSURE HIGH!");
        }
        else
        {
            Debug.Log("Pressure Levels Normal");
        }
    }

    void setColour()
    {
      if (dialRotation < greenThreshold)
        {
  
            pressureColourRingImage.color = Color.green;

        }
      else if ((dialRotation > greenThreshold) && (dialRotation < redThreshold))
        {
   
            pressureColourRingImage.color = Color.yellow;
        }
      else if (dialRotation > redThreshold)
        {
            pressureColourRingImage.color = Color.red;
        }
    }

    IEnumerator DamagePlayer(float seconds)
    {
        while (dialRotation > redThreshold)
        {
            yield return new WaitForSeconds(seconds);
            if (dialRotation > redThreshold) { playerDamagable.TakeDamage(GetComponent<Damager>(), false); }
        }

    }

    IEnumerator Blink(float seconds)
    {
        pressureColourRing.SetActive(false);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(true);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(false);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(true);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(false);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(true);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(false);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(true);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(false);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(true);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(false);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(true);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(false);
        yield return new WaitForSeconds(seconds);
        pressureColourRing.SetActive(true);
        yield return new WaitForSeconds(seconds);
    }

    public void upgradePressureResistance()
    {
        gaugeMultiplier = gaugeMultiplierForEachUpgrade[PlayerInventory.GetNumberOfItem("pressureUpgrade")];
    }
}
