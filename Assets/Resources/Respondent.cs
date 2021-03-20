// using System.Xml.Serialization;
// XmlSerializer serializer = new XmlSerializer(typeof(Respondent));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (Respondent)serializer.Deserialize(reader);
// }

using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot(ElementName = "frame")]
public class Frame
{

	[XmlElement(ElementName = "id")]
	public int Id { get; set; }

	[XmlElement(ElementName = "event")]
	public string Event { get; set; }

	[XmlElement(ElementName = "num_attempt")]
	public int NumAttempt { get; set; }

	[XmlElement(ElementName = "num_block")]
	public int NumBlock { get; set; }

	[XmlElement(ElementName = "time_from_start")]
	public double TimeFromStart { get; set; }

	[XmlElement(ElementName = "eye_on_prediction")]
	public bool EyeOnPrediction { get; set; }

	[XmlElement(ElementName = "eye_on_keyboard")]
	public bool EyeOnKeyboard { get; set; }

	[XmlElement(ElementName = "gesture_executing")]
	public bool GestureExecuting { get; set; }
}

[XmlRoot(ElementName = "attempt")]
public class Attempt
{
    public Attempt()
    {
		Frame = new List<Frame>();
    }

    [XmlElement(ElementName = "frame")]
	public List<Frame> Frame { get; set; }
}

[XmlRoot(ElementName = "block")]
public class Block
{
    public Block()
    {
		Attempt = new List<Attempt>();
    }

	public Block(int attemptCount) : this()
	{
		for (int i = 0; i < attemptCount; ++i)
		{
			Attempt.Add(new Attempt());
		}
	}

	[XmlElement(ElementName = "attempt")]
	public List<Attempt> Attempt { get; set; }
}

[XmlRoot(ElementName = "respondent")]
public class Respondent
{
    public Respondent()
    {
		Block = new List<Block>();
    }

	public Respondent(int blocksCount, int attemptCount) : this()
    {
		for(int i = 0; i < blocksCount; ++i)
        {
			Block.Add(new Block(attemptCount));
        }
    }

    [XmlElement(ElementName = "block")]
	public List<Block> Block { get; set; }

	public List<Frame> this[int i, int j]
    {
		get
        {
			return Block[i].Attempt[j].Frame;
        }
    }
}

