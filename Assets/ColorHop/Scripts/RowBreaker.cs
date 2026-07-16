using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RowBreaker : MonoBehaviour
{
    public RectTransform ParticleParent;
    public int ParticlesPerBlock = 6;
    public float ParticleLifetime = 0.6f;
    public float ParticleSpreadRadius = 220f;
    public float ParticleSize = 40f;

    private Queue<Image> particlePool = new Queue<Image>();

    public void BreakRow(RowContainer row, int matchedColumn)
    {
        Debug.Assert(row != null, "BreakRow called with null row!");
        Debug.Assert(ParticleParent != null, "ParticleParent not assigned!");

        Vector2 rowPos = row.Rect.anchoredPosition;
        float leftEdge = -row.Cells.Length * (row.Rect.sizeDelta.x / row.Cells.Length) * 0.5f;
        float cellW = row.Rect.sizeDelta.x / row.Cells.Length;

        for (int i = 0; i < row.Cells.Length; i++)
        {
            Vector2 cellPos = new Vector2(rowPos.x + leftEdge + cellW * 0.5f + i * cellW, rowPos.y);
            SpawnParticles(cellPos, row.Cells[i].ColorIndex);
        }

        row.Rect.DOScale(Vector3.zero, 0.18f).SetEase(Ease.InBack).OnComplete(() =>
        {
            row.Rect.localScale = Vector3.one;
        });
    }

    private void SpawnParticles(Vector2 origin, int colorIndex)
    {
        Color color = ColorPalette.Colors[colorIndex];
        for (int i = 0; i < ParticlesPerBlock; i++)
        {
            Image p = GetParticle();
            RectTransform prt = p.rectTransform;
            prt.anchoredPosition = origin;
            prt.localScale = Vector3.one;
            p.color = color;

            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float dist = Random.Range(ParticleSpreadRadius * 0.5f, ParticleSpreadRadius);
            Vector2 target = origin + new Vector2(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist);

            Sequence seq = DOTween.Sequence();
            seq.Append(prt.DOAnchorPos(target, ParticleLifetime).SetEase(Ease.OutQuad));
            seq.Join(prt.DOScale(0f, ParticleLifetime).SetEase(Ease.InQuad));
            seq.Join(p.DOFade(0f, ParticleLifetime).SetEase(Ease.InQuad));
            Image capturedP = p;
            seq.OnComplete(() => { ReturnParticle(capturedP); });
        }
    }

    private Image GetParticle()
    {
        if (particlePool.Count > 0)
        {
            Image p = particlePool.Dequeue();
            p.gameObject.SetActive(true);
            p.color = new Color(1f, 1f, 1f, 1f);
            return p;
        }

        GameObject go = new GameObject("Particle");
        go.transform.SetParent(ParticleParent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(ParticleSize, ParticleSize);
        Image img = go.AddComponent<Image>();
        img.raycastTarget = false;
        return img;
    }

    private void ReturnParticle(Image p)
    {
        p.gameObject.SetActive(false);
        particlePool.Enqueue(p);
    }
}
