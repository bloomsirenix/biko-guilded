using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Biko
{
    public class ChatML
    {
     
        public static float CheckMessage(string message)
        {
            if (message == null)
            {
                return float.NaN;

            }
            else
            {
                //Load sample data
                var sampleData = new ToxicityModel.ModelInput()
                {
                    Rev_id = 2.429755E+08F,
                    Comment = message,
                    Year = 2005F,
                    Logged_in = false,
                    Ns = @"article",
                    Sample = @"random",
                    Split = @"train",
                };

                //Load model and predict output
                var result = ToxicityModel.Predict(sampleData);
                return result.Score[1];
            }

        }
    }
}
