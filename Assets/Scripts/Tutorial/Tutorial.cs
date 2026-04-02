using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private TutorialData data;
    [SerializeField] private Player player;


    private Tween _showDialogueTween;
    private string _content;

    


    private void Start()
    {
        player.SetCanPlay(false);
        Show(data);
    }


    private void Update()
    {
        
        
    }



    private Tween ShowSentence(TutorialSentence sentence)
    {
        _content = sentence.Sentence;
       
        _showDialogueTween = DOTween.To(
            () => string.Empty, // Gia tri khoi dau
            x => contentText.text = x, // Gia tri trong thoi gian Tween
            _content, // Gia tri cuoi cung
            4f); // Thoi gian thuc hien Tween

        return _showDialogueTween;
           
    }

    public void Show(TutorialData data)
    {
        canvasGroup.alpha = 1;

        if (data == null || data.sentenceList == null || data.sentenceList.Count <= 0)
        {
            return;
        }

        var sentences = data.sentenceList;
        var sequence = DOTween.Sequence();
        for (var i = 0; i < sentences.Count; i++)
        {
            sequence.Append(ShowSentence(sentences[i]));
        }
        sequence.OnComplete(Hide);
        
    }

    public void Hide()
    {
        
        canvasGroup.alpha = 0;
        player.SetCanPlay(true);
    }

}
