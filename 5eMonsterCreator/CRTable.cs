using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eMonsterCreator
{
    class CRTable
    {
        //This class has no reason to ever change once instantiated, and changing it would cause problems.
        //Therefore this class has no reasonable method to ever change. It is immutable.
        //Its member variables are private and its only member methods are the constructor and getters.

        private CRStats[] table;
        private String[] sizeTable;
        private int tableCount = 34;
        private int excess = 30;

        public CRTable()
        {
            sizeTable = new String[6];

            sizeTable[0] = "Tiny d4";
            sizeTable[1] = "Small d6";
            sizeTable[2] = "Medium d8";
            sizeTable[3] = "Large d10";
            sizeTable[4] = "Huge d12";
            sizeTable[5] = "Gargantuan d20";

            table = new CRStats[tableCount+excess];

            table[0] = new CRStats("0", 2, 0, 6, 0, 0, 0);
            table[1] = new CRStats("1/8", 2, 13, 35, 3, 3, 13);
            table[2] = new CRStats("1/4", 2, 13, 49, 3, 5, 13);
            table[3] = new CRStats("1/2", 2, 13, 70, 3, 8, 13);
            table[4] = new CRStats("1", 2, 13, 85, 3, 14, 13);
            table[5] = new CRStats("2", 2, 13, 100, 3, 20, 13);
            table[6] = new CRStats("3", 2, 13, 115, 4, 26, 13);
            table[7] = new CRStats("4", 2, 14, 130, 5, 32, 14);
            table[8] = new CRStats("5", 3, 15, 145, 6, 38, 15);
            table[9] = new CRStats("6", 3, 15, 160, 6, 44, 15);
            table[10] = new CRStats("7", 3, 15, 175, 6, 50, 15);
            table[11] = new CRStats("8", 3, 16, 190, 7, 56, 16);
            table[12] = new CRStats("9", 4, 16, 205, 7, 62, 16);
            table[13] = new CRStats("10", 4, 17, 220, 7, 68, 16);
            table[14] = new CRStats("11", 4, 17, 235, 8, 74, 17);
            table[15] = new CRStats("12", 4, 17, 250, 8, 80, 17);
            table[16] = new CRStats("13", 5, 18, 265, 8, 86, 18);
            table[17] = new CRStats("14", 5, 18, 280, 8, 92, 18);
            table[18] = new CRStats("15", 5, 18, 295, 8, 98, 18);
            table[19] = new CRStats("16", 5, 18, 310, 9, 104, 18);
            table[20] = new CRStats("17", 6, 19, 325, 10, 110, 19);
            table[21] = new CRStats("18", 6, 19, 340, 10, 116, 19);
            table[22] = new CRStats("19", 6, 19, 355, 10, 122, 19);
            table[23] = new CRStats("20", 6, 19, 400, 10, 140, 19);
            table[24] = new CRStats("21", 7, 19, 445, 11, 158, 20);
            table[25] = new CRStats("22", 7, 19, 490, 11, 176, 20);
            table[26] = new CRStats("23", 7, 19, 535, 11, 194, 20);
            table[27] = new CRStats("24", 7, 19, 580, 12, 212, 21);
            table[28] = new CRStats("25", 8, 19, 625, 12, 230, 21);
            table[29] = new CRStats("26", 8, 19, 670, 12, 248, 21);
            table[30] = new CRStats("27", 8, 19, 715, 13, 266, 22);
            table[31] = new CRStats("28", 8, 19, 760, 13, 284, 22);
            table[32] = new CRStats("29", 9, 19, 805, 13, 302, 22);
            table[33] = new CRStats("30", 9, 19, 850, 14, 320, 23);

            //NEW DATA

            for (int i = 0; i < excess; i++)
            {
                int count = 31 + i;
                table[34 + i] = new CRStats(count.ToString(), 9, 19, 900 + (i * 50), 14, 340 + (i * 20), 23);
            }

            int a = 0;

        }

        public String[] allSizes
        {
            get
            {
                int length = sizeTable.Length;

                if (length == 0)
                {
                    return null;
                }

                String[] sizes = new String[length];

                for (int i = 0; i < length; i++)
                {
                    sizes[i] = sizeTable[i];
                }

                return sizes;
            }
        }

        public int CRTableCount
        {
            get { return tableCount; }
        }

        //This property can return a null if something happens to the table.
        //It's not a concern, but it is a possibility.
        public String[] allStrCRs
        {
            get
            {
                int length = table.Length;

                if(length==0)
                {
                    return null;
                }

                String[] CRs = new String[length];

                for (int i = 0; i < length; i++)
                {
                    CRs[i] = table[i].CR;
                }

                return CRs;
            }
        }

        //This property can return null if there is something wrong with the table.
        //It's not a concern, but it is a possibility.
        public decimal[] allDcmCRs
        {
            get
            {
                int length = table.Length;

                decimal[] CRs = new decimal[length];

                for (int i = 0; i < length; i++)
                {
                    if(!InputParser.tryParseCR(table[i].CR, out CRs[i]))
                    {
                        return null;
                    }
                }

                return CRs;
            }
        }

        //This method can return null.
        //Always check for null.
        public CRStats getStatsByCR(String _CR)
        {
            for(int i=0;i<table.Length;i++)
            {
                CRStats stats = table[i];
                String CR = stats.CR;

                if(CR.Equals(_CR, StringComparison.Ordinal))
                {
                    return stats;
                }
            }

            return null;
        }

        //This method can return null.
        //Always check for null.
        public CRStats getStatsByHP(decimal _HP)
        {
            for (int i = 0; i < table.Length; i++)
            {
                CRStats stats = table[i];
                int maxHP = stats.maxHP;

                //If _HP falls within the current element's maxHP range.
                //This is inclusive, so it matches our maxHP ranges per CR.
                if (maxHP>=_HP)
                {
                    return stats;
                }
            }

            return null;
        }


        //This method can return null.
        //Always check for null.
        public CRStats getStatsByAC(decimal _AC)
        {
            _AC = (int)_AC;
            int length = table.Length;
            for (int i = 0; i<length; i++)
            {
                CRStats stats = table[i];
                int AC = stats.AC;

                if (AC == _AC)
                {
                    return stats;
                }
            }

            if (length >= 1)
            {
                return table[0];
            }

            return null;
        }

        //This method can return null.
        //Always check for null.
        public CRStats getStatsMatchingACandHP(decimal _AC, decimal _HP)
        {
            _AC = (int)_AC;
            for (int i = 0; i < table.Length; i++)
            {
                CRStats stats = table[i];
                int AC = stats.AC;
                int maxHP = stats.maxHP;

                //If _HP falls within the current element's maxHP range.
                //This is inclusive, so it matches our maxHP ranges per CR.
                if(maxHP >= _HP)
                {
                    if(AC==_AC)
                    {
                        return stats;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        //This method can return null.
        //Always check for null.
        public CRStats getStatsMatchingABandDPRandSDC(decimal _AB, decimal _DPR, decimal _SDC)
        {
            _AB = (int)_AB;
            _SDC = (int)_SDC;
            for (int i = 0; i < table.Length; i++)
            {
                CRStats stats = table[i];
                int AB = stats.attackBonus;
                int maxDPR = stats.maxDPR;
                int SDC = stats.saveDC;

                //If _HP falls within the current element's maxHP range.
                //This is inclusive, so it matches our maxHP ranges per CR.
                if (maxDPR >= _DPR)
                {
                    if (AB == _AB && SDC == _SDC)
                    {
                        return stats;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        //This method can return null.
        //Always check for null.
        public CRStats getStatsByDPR(decimal _DPR)
        {
            for (int i = 0; i < table.Length; i++)
            {
                CRStats stats = table[i];
                int maxDPR = stats.maxDPR;

                //If _DPR falls within the current element's maxDPR range.
                //This is inclusive, so it matches our maxDPR ranges per CR.
                if (maxDPR >= _DPR)
                {
                    return stats;
                }
            }

            return null;
        }


        //This method can return null.
        //Always check for null.
        public CRStats getStatsByAB(decimal _AB)
        {
            _AB = (int)_AB;
            int length = table.Length;
            for (int i = 0; i < length; i++)
            {
                CRStats stats = table[i];
                int AB = stats.attackBonus;

                if (AB == _AB)
                {
                    return stats;
                }
            }

            if (length >= 1)
            {
                return table[0];
            }

            return null;
        }

        //This method can return null.
        //Always check for null.
        public CRStats getStatsBySDC(decimal _SDC)
        {
            _SDC = (int)_SDC;
            int length = table.Length;
            for (int i = 0; i < length; i++)
            {
                CRStats stats = table[i];
                int SDC = stats.saveDC;

                if (SDC == _SDC)
                {
                    return stats;
                }
            }

            if (length >= 1)
            {
                return table[0];
            }

            return null;
        }
    }
}
