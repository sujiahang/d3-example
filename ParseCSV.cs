using System;
using System.Data;
using System.Collections;
using System.Text;


namespace SuperMyWay.MyFund.Framework.Utility
{

    /// <summary>
    /// Summary description for ParseCSV
    /// </summary>
    public class ParseCSV
    {
        public ParseCSV()
        {
            
        }

        /// <summary>
        /// Parse the lines from csv file
        /// </summary>
        /// <param name="strInputString"></param>
        /// <returns></returns>
        public static ArrayList Parse(string strInputString)
        {
            int intCounter = 0, intLenght;
            StringBuilder strElem = new StringBuilder();
            ArrayList alParsedCsv = new ArrayList();
            intLenght = strInputString.Length;
            strElem = strElem.Append("");
            int intCurrState = 0;
            int[][] aActionDecider = new int[9][];

            //Build the state array
            /* State > Action
              * 0 > Add Tempstr to arraylist,clear tempstr
              * 1,3,4 > Append input to tempstr
              * 2 > Do nothing
              * 5 > Add Tempstr to arraylist
              * 6 > Unexpected Input, clear arraylist to indicate error
              * 7 > Remove last character in tempstr,add tempstr to arraylist, set cur_state to 5
              * 8 > Remove last character in tempstr,add tempstr to arraylist, set cur_stat to 0
              */
            aActionDecider[0] = new int[4] { 2, 0, 1, 5 };
            aActionDecider[1] = new int[4] { 6, 0, 1, 5 };
            aActionDecider[2] = new int[4] { 4, 3, 3, 6 };
            aActionDecider[3] = new int[4] { 4, 3, 3, 6 };
            aActionDecider[4] = new int[4] { 2, 8, 6, 7 };
            aActionDecider[5] = new int[4] { 5, 5, 5, 5 };
            aActionDecider[6] = new int[4] { 6, 6, 6, 6 };
            aActionDecider[7] = new int[4] { 5, 5, 5, 5 };
            aActionDecider[8] = new int[4] { 0, 0, 0, 0 };

            for (intCounter = 0; intCounter < intLenght; intCounter++)
            {
                intCurrState = aActionDecider[intCurrState][GetInputID(strInputString[intCounter])];
                //take the necessary action depending upon the state returned
                PerformAction(ref intCurrState, strInputString[intCounter], ref strElem, ref alParsedCsv);
            }
            intCurrState = aActionDecider[intCurrState][3];
            PerformAction(ref intCurrState, '\0', ref strElem, ref alParsedCsv);
            return alParsedCsv;
        }


        /// <summary>
        /// check if the character is " or ,
        /// </summary>
        /// <param name="chrInput"></param>
        /// <returns></returns>
        private static int GetInputID(char chrInput)
        {
            if (chrInput == '"')
            {
                return 0;
            }
            else if (chrInput == ',')
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        /// <summary>
        /// check the line, and get the split them to arraylist
        /// </summary>
        /// <param name="intCurrState"></param>
        /// <param name="chrInputChar"></param>
        /// <param name="strElem"></param>
        /// <param name="alParsedCsv"></param>
        private static void PerformAction(ref int intCurrState, char chrInputChar, ref StringBuilder strElem, ref ArrayList alParsedCsv)
        {
            string strTemp = null;
            switch (intCurrState)
            {
                case 0:
                    //Seperate out value to array list
                    strTemp = strElem.ToString();
                    alParsedCsv.Add(strTemp);
                    strElem = new StringBuilder();
                    break;
                case 1:
                case 3:
                case 4:
                    //accumulate the character
                    strElem.Append(chrInputChar);
                    break;
                case 5:
                    //End of line reached. Seperate out value to array list
                    strTemp = strElem.ToString();
                    alParsedCsv.Add(strTemp);
                    break;
                case 6:
                    //Erroneous input. Reject line.
                    alParsedCsv.Clear();
                    break;
                case 7:
                    //wipe ending " and Seperate out value to array list
                    strElem.Remove(strElem.Length - 1, 1);
                    strTemp = strElem.ToString();
                    alParsedCsv.Add(strTemp);
                    strElem = new StringBuilder();
                    intCurrState = 5;
                    break;
                case 8:
                    //wipe ending " and Seperate out value to array list
                    strElem.Remove(strElem.Length - 1, 1);
                    strTemp = strElem.ToString();
                    alParsedCsv.Add(strTemp);
                    strElem = new StringBuilder();
                    //goto state 0
                    intCurrState = 0;
                    break;
            }
        }

    }
}