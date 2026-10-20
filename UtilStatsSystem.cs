using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Util__StatsSystem : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] GameObject HPBar;
    [SerializeField] TextMeshProUGUI HPNum;
    public int MaxHP = 1000;
    [Range(0, 1000)] public int HP = 1000;
    public int DEF;

    [Header("Reference")]
    [SerializeField] GameObject StatsUI;
    [SerializeField] GameObject DeathParticle;
    [SerializeField] Material DamageMatFX;


    [Header("Properties")]
    [SerializeField] UnityEvent OnDeath;
    [SerializeField] bool DoDamage = true;
    [SerializeField] bool DoDeath = true;
    [SerializeField] bool DoDamageMaterialEffect = true;
    [SerializeField] bool DoDeathEffect;


    [Header("State")]
    public bool Damaged;

    [Header("Debug")]
    [SerializeField] private bool DamageEntity;

    //Data
    private List<Material> normal_mat = new();
    private List<Renderer> render = new();
    private bool IsDead;
    private Coroutine c_dmgEffect;

    // UI bar caching
    private RectTransform hpBarRect;
    private float hpBarFullWidth = 0f;
    private float hpBarFullScaleX = 1f;

    //Other thinbg
    bool isUIShown;
    bool isUIShown_prev=true;

    public void RefillHP()
    {
        HP = MaxHP;
    }

    void Start()
    {
        HP = MaxHP;
        if (HPBar != null)
        {
            hpBarRect = HPBar.GetComponent<RectTransform>();
            if (hpBarRect != null)
            {
                hpBarFullWidth = hpBarRect.rect.width;
            }
            else
            {
                hpBarFullScaleX = HPBar.transform.localScale.x;
            }
        }
        StatsUI.SetActive(false);
    }

    void UpdateStats()
    {
        if (HPNum != null) HPNum.text = "HP: " + HP.ToString() + "/" + MaxHP.ToString();
        if (HPBar != null)
        {
            float proportion = MaxHP > 0 ? Mathf.Clamp01((float)HP / (float)MaxHP) : 0f;

            if (hpBarRect != null)
            {
                float newWidth = hpBarFullWidth * proportion;
                hpBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
            }
            else
            {
                Vector3 s = HPBar.transform.localScale;
                s.x = hpBarFullScaleX * proportion;
                HPBar.transform.localScale = s;
            }
        }
    }


    void Update()
    {
        if (DamageEntity)
        {
            TakeDamage(1);
            DamageEntity = false;
        }

        //Death Detection
        if (HP <= 0 && !IsDead)
        {
            if (DoDeathEffect) Instantiate(DeathParticle, transform.position, Quaternion.identity);
            if (DoDeath)
            {
                Destroy(gameObject, 0.1f);
            }
            else
            {
                OnDeath?.Invoke();
            }
            IsDead = true;
        }


        //HP Bar Display
        if (HP != MaxHP)
        {
            isUIShown = true;
            UpdateStats();
        }
        else
        {
            isUIShown = false;
        }


        //UI only appear if damaged
        if (isUIShown != isUIShown_prev)
        {
            Debug.Log("value changed");
            StatsUI.SetActive(isUIShown);
            isUIShown_prev = isUIShown;
        }
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - DEF, 0);
        if (DoDamage)
        {
            if (HP>0) HP -= actualDamage;
            if (DoDamageMaterialEffect)
            {
                if (c_dmgEffect != null)
                {
                    StopCoroutine(DamageEffect());
                    c_dmgEffect = null;
                }
                if (c_dmgEffect == null) StartCoroutine(DamageEffect());
            }
        }
    }

    public void RestoreMaterial()
    {
        //Restore original just incase of a sudden interuption
        for (int i = 0; i < render.Count; i++) render[i].material = normal_mat[i];
        render.Clear();
        normal_mat.Clear();
    }

    IEnumerator DamageEffect()
    {
        Damaged = true;
        //THIS CODE MAY BE UNOPTIMIZED

        //Restore original just incase of a sudden interuption
        for (int i = 0; i < render.Count; i++) render[i].material = normal_mat[i];
        render.Clear();
        normal_mat.Clear();

        //Initialize
        render = GetComponentsInChildren<Renderer>().ToList();
        if (normal_mat.Count < render.Count) normal_mat.AddRange(new Material[render.Count - normal_mat.Count]);

        //Store original materials for backup
        for (int i = 0; i < render.Count; i++) normal_mat[i] = render[i].material;

        //Apply damage material
        foreach (var r in render) r.material = DamageMatFX;
        yield return new WaitForSeconds(0.1f);

        //Restore original materials
        for (int i = 0; i < render.Count; i++) render[i].material = normal_mat[i];
        render.Clear();
        normal_mat.Clear();
        c_dmgEffect = null;
        Damaged = false;
    }
}
