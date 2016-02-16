//also based on these resources
//http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer
//http://answers.unity3d.com/questions/639381/too-much-prefab-for-question-quiz-games.html

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public struct QuestionForSecond
{
    [XmlAttribute("questionText")]
    public string questionText;
    [XmlAttribute("answer1")]
    public string answer1;
    [XmlAttribute("answer2")]
    public string answer2;
    [XmlAttribute("answer3")]
    public string answer3;
    [XmlAttribute("answer4")]
    public string answer4;
    [XmlAttribute("answer5")]
    public string answer5;
    [XmlAttribute("answer6")]
    public string answer6;
    [XmlAttribute("answer7")]
    public string answer7;
    [XmlAttribute("answer8")]
    public string answer8;
    [XmlAttribute("answer9")]
    public string answer9;
    [XmlAttribute("answer10")]
    public string answer10;
    [XmlAttribute("correctAnswer")]
    public string correctAnswer;
    [XmlAttribute("questionScore")]
    public int questionScore;

}

[XmlRoot]
public class QuestionDataForSecond
{
    [XmlArray("QuestionForSecond"), XmlArrayItem("QuestionForSecond")]
    public List<QuestionForSecond> questions = new List<QuestionForSecond>();

    public static QuestionDataForSecond Load(string path)
    {
        try
        {
            var serializer = new XmlSerializer(typeof(QuestionDataForSecond));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as QuestionDataForSecond;
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Exception: " + e);
            return null;
        }
    }

    public static QuestionDataForSecond LoadFromText(string text)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(QuestionDataForSecond));
            return serializer.Deserialize(new StringReader(text)) as QuestionDataForSecond;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Exception: " + e);
            return null;
        }
    }
}
