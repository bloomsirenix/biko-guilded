using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The SentimentIssue class represents a single sentiment record.
/// </summary>
public class SentimentIssue
{
    [LoadColumn(0)] public bool Label { get; set; }
    [LoadColumn(2)] public string Text { get; set; }
}

/// <summary>
/// The SentimentPrediction class represents a single sentiment prediction.
/// </summary>
public class SentimentPrediction
{
    [ColumnName("PredictedLabel")] public bool Prediction { get; set; }
    public float Probability { get; set; }
    public float Score { get; set; }
}