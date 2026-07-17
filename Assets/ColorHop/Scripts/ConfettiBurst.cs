using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ConfettiBurst : MonoBehaviour
{
    public RectTransform ParticleParent;
    public int ParticleCount = 24;
    public float Lifetime = 0.9f;
    public float SpreadRadius = 500f;
    public float MinSize = 18f;
    public float MaxSize = 34f;

    private Queue<Image> pool = new Queue<Image>();

    public void Burst(Vector2 origin)
    {
        Debug.Assert(ParticleParent != null, "ParticleParent not assigned on ConfettiBurst!");

        for (int i = 0; i < ParticleCount; i++)
        {
            Image p = GetParticle();
            RectTransform prt = p.rectTransform;

            float size = Random.Range(MinSize, MaxSize);
            prt.sizeDelta = new Vector2(size, size);
            prt.anchoredPosition = origin;
            prt.localScale = Vector3.one;
            prt.localRotation = Quaternion.identity;

            int colorIdx = ColorPalette.GetRandomIndex();
            p.color = ColorPalette.Colors[colorIdx];

            float angle = Random.Range(200f, 340f) * Mathf.Deg2Rad;
            float dist = Random.Range(SpreadRadius * 0.4f, SpreadRadius);
            Vector2 target = origin + new Vector2(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist + 200f);

            float rotAmount = Random.Range(180f, 720f) * (Random.value > 0.5f ? 1f : -1f);
            float lifetime = Lifetime * Random.Range(0.8f, 1.2f);

            Sequence seq = DOTween.Sequence();
            seq.Append(prt.DOAnchorPos(target, lifetime).SetEase(Ease.OutQuad));
            seq.Join(prt.DORotate(new Vector3(0f, 0f, rotAmount), lifetime, RotateMode.FastBeyond360).SetEase(Ease.Linear));
            seq.Join(p.DOFade(0f, lifetime).SetEase(Ease.InQuad).SetDelay(lifetime * 0.4f));
            Image capturedP = p;
            seq.OnComplete(() => { ReturnParticle(capturedP); });
        }
    }

    private Image GetParticle()
    {
        if (pool.Count > 0)
        {
            Image p = pool.Dequeue();
            p.gameObject.SetActive(true);
            p.color = new Color(1f, 1f, 1f, 1f);
            return p;
        }

        GameObject go = new GameObject("Confetti");
        go.transform.SetParent(ParticleParent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        Image img = go.AddComponent<Image>();
        img.raycastTarget = false;
        return img;
    }

    private void ReturnParticle(Image p)
    {
        p.gameObject.SetActive(false);
        pool.Enqueue(p);
    }
}
