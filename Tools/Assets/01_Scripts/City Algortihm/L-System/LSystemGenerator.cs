using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class LSystemGenerator : MonoBehaviour
{
    public sRule[] rules;
    public string rootSentence;
    [Range(0, 10)]
    public int iterationLimit = 2;

    public bool randomIgnoreRuleModifier = true;
    public float ruleIgnoreChance = 0.3f;

    private void Start()
    {
        Debug.Log(GenerateSentence());
    }

    public string GenerateSentence(string word = null)
    {
        if (word == null)
        {
            word = rootSentence;
        }

        return GrowRecursive(word);
    }

    private string GrowRecursive(string word, int currentIterationIndex = 0)
    {
        // Base Case
        if (currentIterationIndex >= iterationLimit)
        {
            return word;
        }

        StringBuilder newResult = new StringBuilder();

        foreach(char c in word)
        {
            newResult.Append(c);
            ProcessRulesRecursively(newResult, c, currentIterationIndex);
        }

        return newResult.ToString();
    }

    private void ProcessRulesRecursively(StringBuilder newResult, char c, int currentIteration)
    {
        foreach(sRule rule in rules)
        {
            if (rule.letter == c.ToString())
            {
                if (randomIgnoreRuleModifier)
                {
                    if (Random.Range(0f, 1f) <= ruleIgnoreChance)
                    {
                        return;
                    }
                }

                newResult.Append(GrowRecursive(rule.GetResults(), currentIteration + 1));
            }
        }
    }
}
