using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _5eMonsterCreator
{
    public partial class MonsterMakerForm : Form
    {

        const String NAME = "name";

        const String VARS = "vars~";

        const String EHPMULT = "ehpmult";

        const String EHPADD = "ehpadd";
        const String EABADD = "eabadd";
        const String EACADD = "eacadd";

        const String AACADD = "aacadd";
        const String AACDEXADD = "aacdexadd";

        const String STRMOD = "str_mod";
        const String DEXMOD = "dex_mod";
        const String CONMOD = "con_mod";
        const String INTMOD = "int_mod";
        const String WISMOD = "wis_mod";
        const String CHAMOD = "cha_mod";

        const String DESIREDCR = "cr";

        const String CR1_4 = "cr1-4";
        const String CR5_10 = "cr5-10";
        const String CR11_16 = "cr11-16";
        const String CR17_30 = "cr17-30";

        const String PLAYERLEVEL = "pcl";

        const String PLLESSEQU10 = "pcl<=10";
        const String PLGREAT10 = "pcl>10";

        const String MULT = "*";
        const String DIV = "/";
        const String ADD = "+";
        const String SUB = "-";

        const String EQU = "=";
        const String COL = ":";

        DataTable expressionEvaluator;

        CRTable statTable;
        String[] allStrCRs;
        decimal[] allDcmCRs;

        String[] allSizes;

        MonsterTraitTable monTable;
        String[] monTraitNames;

        decimal dcmDesiredCR;
        String strDesiredCR;

        decimal dcmDesiredDefensiveCR;
        String strDesiredDefensiveCR;

        decimal dcmDesiredOffensiveCR;
        String strDesiredOffensiveCR;

        bool validCR = false;
        bool validDefensiveCR = false;
        bool validOffensiveCR = false;

        List<MonsterTrait> chosenTraits;

        int diceSize = 4;
        decimal averageDiceValue = 2.5m;

        public MonsterMakerForm()
        {
            InitializeComponent();

            expressionEvaluator = new DataTable();

            statTable = new CRTable();
            allStrCRs = statTable.allStrCRs;
            allDcmCRs = statTable.allDcmCRs;

            chosenTraits = new List<MonsterTrait>();

            if(allStrCRs==null || allDcmCRs==null)
            {
                Console.WriteLine("Bad table data.");
            }

            for (int i=0;i<allStrCRs.Length;i++)
            {
                if (i < statTable.CRTableCount)
                {
                    CRBox.Items.Add(allStrCRs[i]);
                }
                defensiveCRBox.Items.Add(allStrCRs[i]);
                offensiveCRBox.Items.Add(allStrCRs[i]);
            }
            CRBox.SelectedIndex = 0;
            defensiveCRBox.SelectedIndex = 0;
            offensiveCRBox.SelectedIndex = 0;

            allSizes = statTable.allSizes;

            for(int i=0;i<allSizes.Length;i++)
            {
                sizeBox.Items.Add(allSizes[i]);
            }
            sizeBox.SelectedIndex = 0;

            monTable = new MonsterTraitTable();
            monTraitNames = monTable.monsterTraitNames;

            if(monTraitNames==null)
            {
                Console.WriteLine("Bad Monster Property Data.");
            }

            for(int i=0; i<monTraitNames.Length;i++)
            {
                monsterTraitBox.Items.Add(monTraitNames[i]);
            }

            namingBox.Select();

            MonsterTrait trait = new MonsterTrait("No/Light Armor", "aacdexadd~dex_mod");
            chosenTraits.Add(trait);

            processStatisticsAndChosenTraits();
        }

        //NOT EVENTS
        decimal calculateDefensiveCR()
        {
            decimal AC = effectiveACUpDown.Value;
            decimal HP = effectiveHPUpDown.Value;

            CRStats fullMatch = statTable.getStatsMatchingACandHP(AC, HP);

            if (fullMatch != null)
            {
                decimal dcmCR;

                if (InputParser.tryParseCR(fullMatch.CR, out dcmCR))
                {
                    return dcmCR;
                }
                else
                {
                    Console.WriteLine("CRStats found, but invalid CR.");
                }

            }
            else
            {
                CRStats byAC = statTable.getStatsByAC(AC);
                CRStats byHP = statTable.getStatsByHP(HP);

                if (byAC != null && byHP != null)
                {
                    decimal ACCR, HPCR;
                    if (InputParser.tryParseCR(byAC.CR, out ACCR) && InputParser.tryParseCR(byHP.CR, out HPCR))
                    {
                        return averageCR(ACCR, HPCR);
                    }
                    else
                    {
                        Console.WriteLine("Approximate CRStats found, but one or more CR was invalid.");
                    }
                }
                else
                {
                    Console.WriteLine("Unable to find CRStats, likely table is corrupt.");
                }
            }

            return -1;
        }

        decimal calculateOffensiveCR()
        {
            decimal AB = effectiveABUpDown.Value;
            decimal DPR = effectiveDPRUpDown.Value;
            decimal SDC = actualSDCUpDown.Value;

            CRStats fullMatch = statTable.getStatsMatchingABandDPRandSDC(AB, DPR, SDC);

            if (fullMatch != null)
            {
                decimal dcmCR;

                if (InputParser.tryParseCR(fullMatch.CR, out dcmCR))
                {
                    return dcmCR;
                }
                else
                {
                    Console.WriteLine("CRStats found, but invalid CR.");
                }

            }
            else
            {
                CRStats byAB = statTable.getStatsByAB(AB);
                CRStats byDPR = statTable.getStatsByDPR(DPR);
                CRStats bySDC = statTable.getStatsBySDC(SDC);

                if (byAB != null && byDPR != null && bySDC!=null)
                {
                    decimal ABCR, DPRCR, SDCCR;
                    if (InputParser.tryParseCR(byAB.CR, out ABCR) && InputParser.tryParseCR(byDPR.CR, out DPRCR) && InputParser.tryParseCR(bySDC.CR, out SDCCR))
                    {
                        return averageCR(ABCR, DPRCR, SDCCR);
                    }
                    else
                    {
                        Console.WriteLine("Approximate CRStats found, but one or more CR was invalid.");
                    }
                }
                else
                {
                    Console.WriteLine("Unable to find CRStats, likely table is corrupt.");
                }
            }

            return -1;
        }

        void updateCRGUI()
        {
            DRBox.Text = "Approximate Defensive CR = " + calculateDefensiveCR();
            if (DRBox.Text.Length > 32)
            {
                DRBox.Text = DRBox.Text.Substring(0, 32);
            }
            OCRBox.Text = "Approximate Offensive CR = " + calculateOffensiveCR();
            if (OCRBox.Text.Length > 32)
            {
                OCRBox.Text = OCRBox.Text.Substring(0, 32);
            }
            approxCRLabel.Text = "Approximate CR = " + averageCR(calculateDefensiveCR(), calculateOffensiveCR());
            if (approxCRLabel.Text.Length > 22)
            {
                approxCRLabel.Text = approxCRLabel.Text.Substring(0, 22);
            }
        }

        decimal valueToMeetCR(decimal _targetCR, decimal _partialCR)
        {
            return ((_targetCR * 2) - _partialCR);
        }

        decimal valueToMeetCR(decimal _targetCR, decimal _partialCR1, decimal _partialCR2)
        {
            return ((_targetCR * 3) - (_partialCR1 + _partialCR2));
        }

        decimal averageCR(decimal _CR1, decimal _CR2)
        {
            return (_CR1 + _CR2) / 2;
        }

        decimal averageCR(decimal _CR1, decimal _CR2, decimal _CR3)
        {
            return (_CR1 + _CR2 + _CR3) / 3;
        }

        void normalACandHP()
        {
            CRStats ACandHPStats = statTable.getStatsByCR(strDesiredDefensiveCR);

            if(ACandHPStats!=null)
            {
                effectiveHPUpDown.Value = ACandHPStats.maxHP;
                effectiveACUpDown.Value = ACandHPStats.AC;
            }
        }




        decimal targetCRforHP()
        {
            CRStats AC = statTable.getStatsByAC((int)effectiveACUpDown.Value);
            if (AC != null )
            {
                decimal dcmAC;

                if (InputParser.tryParseCR(AC.CR, out dcmAC))
                {

                    return valueToMeetCR(dcmDesiredDefensiveCR, dcmAC);
                }
            }

            Console.WriteLine("Cannot find CRs for given DPR or SDC values.");
            return -1;
        }

        decimal targetCRforAC()
        {
            CRStats HP = statTable.getStatsByHP((int)effectiveHPUpDown.Value);
            if (HP != null)
            {
                decimal dcmHP;

                if (InputParser.tryParseCR(HP.CR, out dcmHP))
                {

                    return valueToMeetCR(dcmDesiredDefensiveCR, dcmHP);
                }
            }

            Console.WriteLine("Cannot find CRs for given DPR or SDC values.");
            return -1;
        }

        decimal targetCRforDPR()
        {
            CRStats AB = statTable.getStatsByAB((int)effectiveABUpDown.Value);
            CRStats SDC = statTable.getStatsBySDC((int)actualSDCUpDown.Value);
            if (AB != null && SDC != null)
            {
                decimal dcmAB, dcmSDC;

                if (InputParser.tryParseCR(AB.CR, out dcmAB) && InputParser.tryParseCR(SDC.CR, out dcmSDC))
                {

                    return valueToMeetCR(dcmDesiredOffensiveCR, dcmAB, dcmSDC);
                }
            }

            Console.WriteLine("Cannot find CRs for given AB or SDC values.");
            return -1;
        }

        void autoHP()
        {
            if (validDefensiveCR)
            {
                CRStats easy = statTable.getStatsByCR(strDesiredDefensiveCR);
                if (easy.AC == effectiveACUpDown.Value)
                {
                    effectiveHPUpDown.Value = easy.maxHP;
                }
                else
                {
                    decimal targetCR = targetCRforHP();
                    if (targetCR < 0)
                    {
                        targetCR = 0;
                    }
                    String strTargetCR = dcmCRtoStrCR(targetCR);

                    CRStats stats = statTable.getStatsByCR(strTargetCR);

                    if (stats != null)
                    {
                        effectiveHPUpDown.Value = stats.maxHP;
                    }
                    else
                    {
                        Console.WriteLine("CR not found. Cannot autofill AB.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Defensive CR, cannot autofill AC.");
            }
        }

        void autoAC()
        {
            if (validDefensiveCR)
            {
                CRStats easy = statTable.getStatsByCR(strDesiredDefensiveCR);
                if (easy.maxHP == effectiveHPUpDown.Value)
                {
                    effectiveACUpDown.Value = easy.AC;
                }
                else
                {
                    decimal targetCR = targetCRforAC();
                    if (targetCR < 0)
                    {
                        targetCR = 0;
                    }
                    String strTargetCR = dcmCRtoStrCR(targetCR);

                    CRStats stats = statTable.getStatsByCR(strTargetCR);

                    if (stats != null)
                    {
                        effectiveACUpDown.Value = stats.AC;
                    }
                    else
                    {
                        Console.WriteLine("CR not found. Cannot autofill AC.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Defensive CR, cannot autofill AC.");
            }
        }


        void autoDPR()
        {
            if (validOffensiveCR)
            {
                CRStats easy = statTable.getStatsByCR(strDesiredOffensiveCR);
                if (easy.attackBonus == effectiveABUpDown.Value && easy.saveDC == actualSDCUpDown.Value)
                {
                    effectiveDPRUpDown.Value = easy.maxDPR;
                }
                else
                {
                    decimal targetCR = targetCRforDPR();
                    if (targetCR < 0)
                    {
                        targetCR = 0;
                    }
                    String strTargetCR = dcmCRtoStrCR(targetCR);

                    CRStats stats = statTable.getStatsByCR(strTargetCR);

                    if (stats != null)
                    {
                        effectiveDPRUpDown.Value = stats.maxDPR;
                    }
                    else
                    {
                        Console.WriteLine("CR not found. Cannot autofill DPR.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Bad Offensive CR. Cannot autofill DPR.");
            }
        }


        void effectiveACtoActualAC(decimal _effectiveAC)
        {

            if (_effectiveAC >= Constants.MAXAC)
            {
                _effectiveAC = Constants.MAXAC;
                effectiveACUpDown.Value = _effectiveAC;
            }

            decimal actualACAdder = actualACAdderUpDown.Value+ dexACAdderUpDown.Value;
            decimal effectiveACAdder = effectiveACAdderUpDown.Value;

            _effectiveAC -= effectiveACAdder;

            if(_effectiveAC> Constants.MAXAC)
            {
                _effectiveAC = Constants.MAXAC;
                decimal temp = _effectiveAC + effectiveACAdder + actualACAdder;
                if (temp < 0)
                {
                    effectiveACUpDown.Value = 0;
                }
                else if (temp > Constants.MAXAC)
                {
                    effectiveACUpDown.Value = Constants.MAXAC;
                }
                else
                {
                    effectiveACUpDown.Value = temp;
                }
            }

            if (_effectiveAC < 0)
            {
                _effectiveAC = 0;
                decimal temp = actualACAdder + effectiveACAdder;
                if (temp < 0)
                {
                    effectiveACUpDown.Value = 0;
                }
                else if (temp > Constants.MAXAC)
                {
                    effectiveACUpDown.Value = Constants.MAXAC;
                }
                else
                {
                    effectiveACUpDown.Value = temp;
                }
            }

            actualACUpDown.Value = _effectiveAC;

            _effectiveAC -= actualACAdder;

            if (_effectiveAC > Constants.MAXAC)
            {
                _effectiveAC = Constants.MAXAC;
                decimal temp = _effectiveAC + effectiveACAdder + actualACAdder;
                if (temp < 0)
                {
                    effectiveACUpDown.Value = 0;
                }
                else if (temp > Constants.MAXAC)
                {
                    effectiveACUpDown.Value = Constants.MAXAC;
                }
                else
                {
                    effectiveACUpDown.Value = temp;
                }
            }

            if (_effectiveAC < 0)
            {
                _effectiveAC = 0;
                decimal temp = actualACAdder + effectiveACAdder;
                if (temp < 0)
                {
                    effectiveACUpDown.Value = 0;
                }
                else if (temp > Constants.MAXAC)
                {
                    effectiveACUpDown.Value = Constants.MAXAC;
                }
                else
                {
                    effectiveACUpDown.Value = temp;
                }
            }

            startingACUpDown.Value = _effectiveAC;
            updateCRGUI();
        }

        void effectiveHPtoActualHP(decimal _effectiveHP)
        {

            if (_effectiveHP>=Constants.MAXHP)
            {
                _effectiveHP = Constants.MAXHP;
                effectiveHPUpDown.Value = _effectiveHP;
            }

            decimal effectiveHPMultiplier = effectiveHPMultiplierUpDown.Value;
            decimal effectiveHPAdder = effectiveHPAdderUpDown.Value;

            _effectiveHP -= effectiveHPAdder;
            _effectiveHP /= effectiveHPMultiplier;

            if (_effectiveHP > Constants.MAXHP)
            {
                _effectiveHP = Constants.MAXHP;
                decimal temp = (1 * effectiveHPMultiplier) + effectiveHPAdder;
                if (temp < 1)
                {
                    effectiveHPUpDown.Value = 1;
                }
                else if (temp > Constants.MAXHP)
                {
                    effectiveHPUpDown.Value = Constants.MAXHP;
                }
                else
                {
                    effectiveHPUpDown.Value = temp;
                }
            }

            if (_effectiveHP<1)
            {
                _effectiveHP = 1;
                decimal temp = (1 * effectiveHPMultiplier) + effectiveHPAdder;
                if (temp < 1)
                {
                    effectiveHPUpDown.Value = 1;
                }
                else if(temp > Constants.MAXHP)
                {
                    effectiveHPUpDown.Value = Constants.MAXHP;
                }
                else
                {
                    effectiveHPUpDown.Value = temp;
                }
            }

            HPUpDown.Value = _effectiveHP;

            decimal constitutionModifier = conModAdderUpDown.Value;
            decimal numberOfHitDice = Math.Ceiling(HPUpDown.Value / (constitutionModifier + averageDiceValue));

            while((constitutionModifier * numberOfHitDice + averageDiceValue * numberOfHitDice) > HPUpDown.Value)
            {
                numberOfHitDice -= 1;
            }

            if (numberOfHitDice == 0)
            {
                numberOfHitDice = 1;
            }

            if ((constitutionModifier * numberOfHitDice + diceSize*numberOfHitDice) < HPUpDown.Value)
            {
                numberOfHitDice += 1;
            }

            if ((constitutionModifier * numberOfHitDice + numberOfHitDice) > HPUpDown.Value)
            {
                HPUpDown.Value = (constitutionModifier * numberOfHitDice) + numberOfHitDice;
                effectiveHPUpDown.Value = (HPUpDown.Value * effectiveHPMultiplier) + effectiveHPAdder;
            }

            hitDiceUpDown.Value = numberOfHitDice;


            diceCodeBox.Text = numberOfHitDice + "d" + diceSize;

            updateCRGUI();
        }

        void effectiveABCalculation()
        {


            decimal temp = actualABAdderUpDown.Value + proficiencyUpDown.Value;

            if (temp > Constants.MAXAB)
            {
                actualABUpDown.Value = Constants.MAXAB;
            }
            else if (temp < 0)
            {
                actualABUpDown.Value = 0;
            }
            else
            {
                actualABUpDown.Value = temp;
            }

            temp = effectiveABAdderUpDown.Value + actualABAdderUpDown.Value + proficiencyUpDown.Value;

            if (temp > Constants.MAXAB)
            {
                effectiveABUpDown.Value = Constants.MAXAB;
            }
            else if(temp< 0)
            {
                effectiveABUpDown.Value = 0;
            }
            else
            {
                effectiveABUpDown.Value = temp;
            }

            updateCRGUI();
        }

        void actualSDCCalculation()
        {
            decimal temp = 8 + actualSDCAdderUpDown.Value + proficiencyUpDown.Value;

            if (temp > Constants.MAXSDC)
            {
                actualSDCUpDown.Value = Constants.MAXSDC;
            }
            else if (temp < 0)
            {
                actualSDCUpDown.Value = 0;
            }
            else
            {
                actualSDCUpDown.Value = temp;
            }


            updateCRGUI();
        }

        String dcmCRtoStrCR(decimal _CR)
        {
            String strCR= _CR.ToString();
            if (_CR > 1)
            {
                _CR = (int)_CR;
                strCR = _CR.ToString();
            }
            else
            {
                if (_CR > .5m)
                {
                    strCR = "1/2";
                }
                else if (_CR > .25m)
                {
                    strCR = "1/4";
                }
                else if (_CR > .125m)
                {
                    strCR = "1/8";
                }
            }

            if (_CR > 60)
            {
                _CR = 60;
                strCR = _CR.ToString();
            }
            return strCR;
        }

        //THESE METHODS ARE STILL NOT EVENTS. THEY PROCESS THE MONSTER TRAITS

        MonsterTrait initialBakeMonsterTrait(MonsterTrait _trait)
        {
            String traitDefinition = _trait.traitDefinition;

            String[] definitionHalves = traitDefinition.Split(new[] {VARS},StringSplitOptions.RemoveEmptyEntries);

            String statements;

            String[] variables;

            if(definitionHalves.Length==1)
            {
                return _trait;
            }
            else if(definitionHalves.Length==2)
            {
                statements = definitionHalves[0];

                variables = definitionHalves[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                Console.WriteLine("Bad trait definition. Cannot parse.");
                return null;
            }

            for(int i=0;i<variables.Length;i++)
            {
                String currentVariable = variables[i];

                decimal value = 0;

                using (ModalInputForm modalForm = new ModalInputForm(variables[i]))
                {
                    var result = modalForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        value = modalForm.inputValue;
                    }
                    else
                    {
                        Console.WriteLine("User cancelled initial bake.");
                        return null;
                    }
                }

                statements = statements.Replace(currentVariable,value.ToString());
            }

            return new MonsterTrait(_trait.traitName, statements);
        }

        String finalBakeMonsterTrait(MonsterTrait _trait)
        {
            String traitDefinition = _trait.traitDefinition;

            if (traitDefinition.Contains(PLGREAT10) || traitDefinition.Contains(PLLESSEQU10))
            {
                pcLevelLabel.ForeColor = Color.Red;
            }

            if (traitDefinition.Contains(CR1_4) || traitDefinition.Contains(CR5_10) || traitDefinition.Contains(CR11_16) || traitDefinition.Contains(CR17_30))
            {
                CRLabel.ForeColor = Color.Red;
            }
            traitDefinition = traitDefinition.Replace(CR1_4, "pr1&4&&&");
            traitDefinition = traitDefinition.Replace(CR5_10, "pr5&10&&&");
            traitDefinition = traitDefinition.Replace(CR11_16, "pr11&16&&&");
            traitDefinition = traitDefinition.Replace(CR17_30, "pr17&30&&&");

            if (traitDefinition.Contains(PLAYERLEVEL))
            {
                traitDefinition = traitDefinition.Replace(PLAYERLEVEL, pcLevelUpDown.Value.ToString());
                pcLevelLabel.ForeColor = Color.Red;
            }

            if (traitDefinition.Contains(DESIREDCR))
            {
                traitDefinition = traitDefinition.Replace(DESIREDCR, dcmDesiredCR.ToString());
                CRLabel.ForeColor = Color.Red;
            }

            if (traitDefinition.Contains(STRMOD))
            {
                traitDefinition = traitDefinition.Replace(STRMOD, STRModUpDown.Value.ToString());
                strModLabel.ForeColor = Color.Red;
            }
            if (traitDefinition.Contains(DEXMOD))
            {
                traitDefinition = traitDefinition.Replace(DEXMOD, DEXModUpDown.Value.ToString());
                dexModLabel.ForeColor = Color.Red;
            }
            if (traitDefinition.Contains(CONMOD))
            {
                traitDefinition = traitDefinition.Replace(CONMOD, CONModUpDown.Value.ToString());
                conModLabel.ForeColor = Color.Red;
            }
            if (traitDefinition.Contains(INTMOD))
            {
                traitDefinition = traitDefinition.Replace(INTMOD, INTModUpDown.Value.ToString());
                intModLabel.ForeColor = Color.Red;
            }
            if (traitDefinition.Contains(WISMOD))
            {
                traitDefinition = traitDefinition.Replace(WISMOD, WISModUpDown.Value.ToString());
                wisModLabel.ForeColor = Color.Red;
            }
            if (traitDefinition.Contains(CHAMOD))
            {
                traitDefinition = traitDefinition.Replace(CHAMOD, CHAModUpDown.Value.ToString());
                chaModLabel.ForeColor = Color.Red;
            }

            traitDefinition = traitDefinition.Replace("pr1&4&&&", CR1_4);
            traitDefinition = traitDefinition.Replace("pr5&10&&&", CR5_10);
            traitDefinition = traitDefinition.Replace("pr11&16&&&", CR11_16);
            traitDefinition = traitDefinition.Replace("pr17&30&&&", CR17_30);

            return traitDefinition;
        }

        void pushTrait(MonsterTrait _trait)
        {
            String traitDefinition = finalBakeMonsterTrait(_trait);

            String[] statements = traitDefinition.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i=0;i<statements.Length;i++)
            {
                String[] conditionals = statements[i].Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                String[] singleStatement = {};

                if(conditionals.Length>0)
                {
                    singleStatement = conditionals[conditionals.Length - 1].Split(new[] { "~" }, StringSplitOptions.RemoveEmptyEntries);
                }

                String expression;

                if (singleStatement.Length == 2)
                {

                    bool canApply = false;

                    if(conditionals.Length==1)
                    {
                        canApply = true;
                    }
                    else if(conditionals[0].Equals(CR1_4,StringComparison.Ordinal) && dcmDesiredCR >= 1 && dcmDesiredCR<=4)
                    {
                        canApply = true;
                    }
                    else if (conditionals[0].Equals(CR5_10, StringComparison.Ordinal) && dcmDesiredCR >= 5 && dcmDesiredCR <= 10)
                    {
                        canApply = true;
                    }
                    else if (conditionals[0].Equals(CR11_16, StringComparison.Ordinal) && dcmDesiredCR >= 11 && dcmDesiredCR <= 16)
                    {
                        canApply = true;
                    }
                    else if (conditionals[0].Equals(CR17_30, StringComparison.Ordinal) && dcmDesiredCR >= 17 && dcmDesiredCR <= 30)
                    {
                        canApply = true;
                    }
                    else if(conditionals[0].Equals(PLLESSEQU10, StringComparison.Ordinal) && pcLevelUpDown.Value<=10)
                    {
                        canApply = true;
                    }
                    else if (conditionals[0].Equals(PLGREAT10, StringComparison.Ordinal) && pcLevelUpDown.Value > 10)
                    {
                        canApply = true;
                    }

                    if (canApply)
                    {
                        try
                        {
                            expression = singleStatement[1];

                            String strValue = expressionEvaluator.Compute(expression, "").ToString();
                            double dblValue;

                            double.TryParse(strValue, out dblValue);

                            decimal value = (decimal)dblValue;

                            String affectedVariable = singleStatement[0];
                            if (affectedVariable.Contains(EHPMULT))
                            {
                                if (value != 0)
                                {
                                    effectiveHPMultiplierUpDown.Value *= value;
                                }
                            }
                            else if (affectedVariable.Contains(EHPADD))
                            {
                                decimal temp = effectiveHPAdderUpDown.Value + value;

                                if (temp > effectiveHPAdderUpDown.Maximum)
                                {
                                    effectiveHPAdderUpDown.Value = effectiveHPAdderUpDown.Maximum;
                                }
                                else if (temp < effectiveHPAdderUpDown.Minimum)
                                {
                                    effectiveHPAdderUpDown.Value = effectiveHPAdderUpDown.Minimum;
                                }
                                else
                                {
                                    effectiveHPAdderUpDown.Value += value;
                                }
                            }
                            else if (affectedVariable.Contains(EABADD))
                            {
                                decimal temp = effectiveABAdderUpDown.Value + value;

                                if (temp > effectiveABAdderUpDown.Maximum)
                                {
                                    effectiveABAdderUpDown.Value = effectiveABAdderUpDown.Maximum;
                                }
                                else if (temp < effectiveABAdderUpDown.Minimum)
                                {
                                    effectiveABAdderUpDown.Value = effectiveABAdderUpDown.Minimum;
                                }
                                else
                                {
                                    effectiveABAdderUpDown.Value += value;
                                }
                            }
                            else if (affectedVariable.Contains(EACADD))
                            {
                                decimal temp = effectiveACAdderUpDown.Value + value;

                                if (temp > effectiveACAdderUpDown.Maximum)
                                {
                                    effectiveACAdderUpDown.Value = effectiveACAdderUpDown.Maximum;
                                }
                                else if (temp < effectiveACAdderUpDown.Minimum)
                                {
                                    effectiveACAdderUpDown.Value = effectiveACAdderUpDown.Minimum;
                                }
                                else
                                {
                                    effectiveACAdderUpDown.Value += value;
                                }
                            }
                            else if (affectedVariable.Contains(AACADD))
                            {
                                decimal temp = actualACAdderUpDown.Value + value;

                                if (temp > actualACAdderUpDown.Maximum)
                                {
                                    actualACAdderUpDown.Value = actualACAdderUpDown.Maximum;
                                }
                                else if (temp < actualACAdderUpDown.Minimum)
                                {
                                    actualACAdderUpDown.Value = actualACAdderUpDown.Minimum;
                                }
                                else
                                {
                                    actualACAdderUpDown.Value += value;
                                }
                            }
                            else if (affectedVariable.Contains(AACDEXADD))
                            {
                                decimal temp = dexACAdderUpDown.Value + value;

                                if ( temp > dexACAdderUpDown.Maximum)
                                {
                                    dexACAdderUpDown.Value = dexACAdderUpDown.Maximum;
                                }
                                else if ( temp < dexACAdderUpDown.Minimum)
                                {
                                    dexACAdderUpDown.Value = dexACAdderUpDown.Minimum;
                                }
                                else
                                {
                                    dexACAdderUpDown.Value += value;
                                }
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Statement in trait definition is malformed.");
                        }
                    }


                }

                
            }

        }

        void processStatisticsAndChosenTraits()
        {
            CRLabel.ForeColor = Control.DefaultForeColor;
            pcLevelLabel.ForeColor = Control.DefaultForeColor;

            strModLabel.ForeColor = Control.DefaultForeColor;
            dexModLabel.ForeColor = Control.DefaultForeColor;
            conModLabel.ForeColor = Control.DefaultForeColor;
            intModLabel.ForeColor = Control.DefaultForeColor;
            wisModLabel.ForeColor = Control.DefaultForeColor;
            chaModLabel.ForeColor = Control.DefaultForeColor;

            effectiveHPAdderUpDown.Value = 0;
            effectiveHPMultiplierUpDown.Value = 1;

            effectiveACAdderUpDown.Value = 0;
            actualACAdderUpDown.Value = 0;
            dexACAdderUpDown.Value = 0;

            effectiveABAdderUpDown.Value = 0;


            for(int i=0;i<chosenTraits.Count;i++)
            {
                pushTrait(chosenTraits[i]);
            }

            updateAttributeValues();

            effectiveHPtoActualHP(effectiveHPUpDown.Value);
            effectiveACtoActualAC(effectiveACUpDown.Value);
            effectiveABCalculation();
            actualSDCCalculation();

        }

        void updateAttributeValues()
        {
            conModAdderUpDown.Value = CONModUpDown.Value;
            conModLabel.ForeColor = Color.Red;
        
            if (strRadio.Checked)
            {
                actualABAdderUpDown.Value = STRModUpDown.Value;

                strModLabel.ForeColor = Color.Red;
            }
            else if(dexRadio.Checked)
            {
                actualABAdderUpDown.Value = DEXModUpDown.Value;
                dexModLabel.ForeColor = Color.Red;
            }
            else if(spellRadio.Checked)
            {
                if(intRadio.Checked)
                {
                    actualABAdderUpDown.Value = INTModUpDown.Value;
                    intModLabel.ForeColor = Color.Red;
                }
                else if(wisRadio.Checked)
                {
                    actualABAdderUpDown.Value = WISModUpDown.Value;
                    wisModLabel.ForeColor = Color.Red;
                }
                else if(chaRadio.Checked)
                {
                    actualABAdderUpDown.Value = CHAModUpDown.Value;
                    chaModLabel.ForeColor = Color.Red;
                }
            }

            if (intRadio.Checked)
            {
                actualSDCAdderUpDown.Value = INTModUpDown.Value;
                intModLabel.ForeColor = Color.Red;
            }
            else if (wisRadio.Checked)
            {
                actualSDCAdderUpDown.Value = WISModUpDown.Value;
                wisModLabel.ForeColor = Color.Red;
            }
            else if (chaRadio.Checked)
            {
                actualSDCAdderUpDown.Value = CHAModUpDown.Value;
                chaModLabel.ForeColor = Color.Red;
            }
        }

        //EVENTS
        private void monsterGenerator_Load(object sender, EventArgs e)
        {

        }


        //CR COMBO BOXES
        private void CRBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            String CR = (String) CRBox.SelectedItem;

            validCR = InputParser.tryParseCR(CR, out dcmDesiredCR);

            if(!validCR)
            {
                CRLabel.Text = "INVALID CR:";
                strDesiredCR = "-1";
            }
            else
            {
                CRLabel.Text = "Desired CR:";
                strDesiredCR = CR;

                CRStats stats = statTable.getStatsByCR(CR);
                proficiencyUpDown.Value = stats.proficiencyBonus;

                defensiveCRBox.SelectedIndex = CRBox.SelectedIndex;
                offensiveCRBox.SelectedIndex = CRBox.SelectedIndex;
            }

            Console.WriteLine("CR = " + dcmDesiredCR);

            effectiveABCalculation();

            processStatisticsAndChosenTraits();
        }

        private void defensiveCRBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String CR= (String)CRBox.SelectedItem;
            String defensiveCR = (String)defensiveCRBox.SelectedItem;

            validDefensiveCR = InputParser.tryParseCR(defensiveCR, out dcmDesiredDefensiveCR);

            if (!validDefensiveCR)
            {
                defensiveCRLabel.Text = "INVALID CR:";
                strDesiredDefensiveCR = "-1";
            }
            else
            {
                defensiveCRLabel.Text = "Desired Defensive CR:";
                strDesiredDefensiveCR = defensiveCR;

                CRStats stats = statTable.getStatsByCR(defensiveCR);
            }

            decimal tempOffensiveCR = valueToMeetCR(dcmDesiredCR,dcmDesiredDefensiveCR);

            for(int i=0;i<allDcmCRs.Length;i++)
            {
                if(tempOffensiveCR==0 && dcmDesiredOffensiveCR<1)
                {
                    break;
                }
                if(allDcmCRs[i]>=tempOffensiveCR)
                {
                    offensiveCRBox.SelectedIndex = i;
                    break;
                }
            }

            Console.WriteLine("Defensive CR = " + dcmDesiredDefensiveCR);
        }

        private void offensiveCRBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String CR = (String)CRBox.SelectedItem;
            String offensiveCR = (String)offensiveCRBox.SelectedItem;

            validOffensiveCR = InputParser.tryParseCR(offensiveCR, out dcmDesiredOffensiveCR);

            if (!validOffensiveCR)
            {
                offensiveCRLabel.Text = "INVALID CR:";
                strDesiredOffensiveCR = "-1";
            }
            else
            {
                offensiveCRLabel.Text = "Desired Offensive CR:";
                strDesiredOffensiveCR = offensiveCR;

                CRStats stats = statTable.getStatsByCR(offensiveCR);
            }

            decimal tempDefensiveCR = valueToMeetCR(dcmDesiredCR,dcmDesiredOffensiveCR);

            for (int i = 0; i < allDcmCRs.Length; i++)
            {
                if (tempDefensiveCR == 0 && dcmDesiredDefensiveCR < 1)
                {
                    break;
                }
                if (allDcmCRs[i] >= tempDefensiveCR)
                {
                    defensiveCRBox.SelectedIndex = i;
                    break;
                }
            }

            Console.WriteLine("Offensive CR = " + dcmDesiredOffensiveCR);
        }

        //TOP ROW
        private void pcLevelUpDown_ValueChanged(object sender, EventArgs e)
        {
            processStatisticsAndChosenTraits();
        }


        private void sizeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(sizeBox.SelectedItem.ToString().Equals(allSizes[0],StringComparison.Ordinal))
            {
                diceSize = 4;
            }
            else if (sizeBox.SelectedItem.ToString().Equals(allSizes[1], StringComparison.Ordinal))
            {
                diceSize = 6;
                averageDiceValue = 3.5m;
            }
            else if (sizeBox.SelectedItem.ToString().Equals(allSizes[2], StringComparison.Ordinal))
            {
                diceSize = 8;
                averageDiceValue = 4.5m;
            }
            else if (sizeBox.SelectedItem.ToString().Equals(allSizes[3], StringComparison.Ordinal))
            {
                diceSize = 10;
                averageDiceValue = 5.5m;
            }
            else if (sizeBox.SelectedItem.ToString().Equals(allSizes[4], StringComparison.Ordinal))
            {
                diceSize = 12;
                averageDiceValue = 6.5m;
            }
            else if (sizeBox.SelectedItem.ToString().Equals(allSizes[5], StringComparison.Ordinal))
            {
                diceSize = 20;
                averageDiceValue = 10.5m;
            }

            effectiveHPtoActualHP(effectiveHPUpDown.Value);
            effectiveACtoActualAC(effectiveACUpDown.Value);
        }


        //AUTO BUTTONS

        private void autoAllButton_Click(object sender, EventArgs e)
        {
            autoDefButton.PerformClick();
            autoOffButton.PerformClick();
        }

        //DEF AUTOBUTTONS
        private void autoDefButton_Click(object sender, EventArgs e)
        {
            normalACandHP();
        }

        private void autoAC_Click(object sender, EventArgs e)
        {
            autoAC();
        }

        private void autoHPButton_Click(object sender, EventArgs e)
        {
            autoHP();
        }

        //OFF AUTOBUTTONS
        private void autoOffButton_Click(object sender, EventArgs e)
        {
            autoDPR();
        }
        private void autoDPRButton_Click(object sender, EventArgs e)
        {
            autoDPR();
        }


        //ARMOR BUTTON
        private void armorButton_Click(object sender, EventArgs e)
        {
            MonsterTrait trait = new MonsterTrait("Armor", "aacadd~AC_From_Armor aacdexadd~IIF(IIF(dex_mod<Maximum_Allowed_Dexterity_Bonus,dex_mod,Maximum_Allowed_Dexterity_Bonus)>Minimum_Allowed_Dexterity_Bonus,IIF(dex_mod<Maximum_Allowed_Dexterity_Bonus,dex_mod,Maximum_Allowed_Dexterity_Bonus),Minimum_Allowed_Dexterity_Bonus) vars~AC_From_Armor,Maximum_Allowed_Dexterity_Bonus,Minimum_Allowed_Dexterity_Bonus");
            trait = initialBakeMonsterTrait(trait);

            if(trait!=null)
            {
                chosenTraits[0] = trait;
            }

            processStatisticsAndChosenTraits();
        }

        //DR UPDOWNS


        private void effectiveHPUpDown_ValueChanged(object sender, EventArgs e)
        {
            processStatisticsAndChosenTraits();
        }

        private void effectiveACUpDown_ValueChanged(object sender, EventArgs e)
        {
            processStatisticsAndChosenTraits();
        }

        private void HPMultiplierUpDown_ValueChanged(object sender, EventArgs e)
        {
            effectiveHPUpDown.Increment = effectiveHPMultiplierUpDown.Value;
        }

        //OCR UPDOWNS



        private void proficiencyUpDown_ValueChanged(object sender, EventArgs e)
        {
            secondProficiencyUpDown.Value = proficiencyUpDown.Value;
            processStatisticsAndChosenTraits();
        }

        private void effectiveDPRUpDown_ValueChanged(object sender, EventArgs e)
        {
            processStatisticsAndChosenTraits();
        }

        //OCR RADIOS

        private void strRadio_CheckedChanged(object sender, EventArgs e)
        {
            abModiferLabel.Text = "STR MOD:";
            processStatisticsAndChosenTraits();
        }

        private void dexRadio_CheckedChanged(object sender, EventArgs e)
        {
            abModiferLabel.Text = "DEX MOD:";
            processStatisticsAndChosenTraits();
        }

        private void spellRadio_CheckedChanged(object sender, EventArgs e)
        {
            if(intRadio.Checked)
            {
                abModiferLabel.Text = "INT MOD:";
            }
            else if (wisRadio.Checked)
            {
                abModiferLabel.Text = "WIS MOD:";
            }
            else if (chaRadio.Checked)
            {
                abModiferLabel.Text = "CHA MOD:";
            }
            processStatisticsAndChosenTraits();
        }

        //SPELLCASTING MOD RADIOS

        private void intRadio_CheckedChanged_1(object sender, EventArgs e)
        {
            sdcModifierLabel.Text = "INT MOD:";
            if (spellRadio.Checked)
            {
                abModiferLabel.Text = "INT MOD:";
            }
            processStatisticsAndChosenTraits();

        }

        private void wisRadio_CheckedChanged(object sender, EventArgs e)
        {

            sdcModifierLabel.Text = "WIS MOD:";
            if (spellRadio.Checked)
            {
                abModiferLabel.Text = "WIS MOD:";
            }
            processStatisticsAndChosenTraits();

        }

        private void chaRadio_CheckedChanged(object sender, EventArgs e)
        {

            sdcModifierLabel.Text = "CHA MOD:";
            if (spellRadio.Checked)
            {
                abModiferLabel.Text = "CHA MOD:";
            }
            processStatisticsAndChosenTraits();

        }

        //MONSTER PROPERTIES
        private void monsterPropBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MonsterTrait monTrait = monTable.getMonsterTraitByTraitName((String)monsterTraitBox.SelectedItem);

            if (monTrait != null)
            {
                monsterPropDescriptionBox.Text = monTrait.traitDefinition;
            }
        }

        private void chosenMonPropBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String strMonTrait = (String)chosenMonTraitBox.SelectedItem;

            for(int i=0; i<chosenTraits.Count;i++)
            {
                if(chosenTraits[i].traitName.Equals(strMonTrait, StringComparison.Ordinal))
                {
                    monsterPropDescriptionBox.Text = chosenTraits[i].traitDefinition;
                }
            }
        }

        private void monsterTraitBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                selectButton.PerformClick();
            }
        }

        private void chosenMonTraitBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                removeButton.PerformClick();
            }
        }

        private void monsterPropBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            selectButton.PerformClick();
        }

        private void chosenMonPropBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            removeButton.PerformClick();
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            MonsterTrait monTrait = monTable.getMonsterTraitByTraitName((String)monsterTraitBox.SelectedItem);

            if (monTrait != null)
            {
                if (!chosenMonTraitBox.Items.Contains(monTrait.traitName))
                {

                    MonsterTrait bakedTrait = initialBakeMonsterTrait(monTrait);

                    if (bakedTrait != null)
                    {
                        chosenMonTraitBox.Items.Add(bakedTrait.traitName);
                        chosenTraits.Add(bakedTrait);
                        processStatisticsAndChosenTraits();
                    }
                }
            }


        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            String traitName = (String)chosenMonTraitBox.SelectedItem;
            for(int i=0;i<chosenTraits.Count;i++)
            {
                MonsterTrait checkingTrait = chosenTraits[i];
                if(checkingTrait.traitName.Equals(traitName, StringComparison.Ordinal))
                {
                    chosenMonTraitBox.Items.Remove(traitName);
                    chosenTraits.Remove(checkingTrait);
                    processStatisticsAndChosenTraits();
                    break;
                }
            }
        }

        //THE ATTRIBUTE MOD UPDOWNS

        private void STRModUpDown_ValueChanged(object sender, EventArgs e)
        {
            processStatisticsAndChosenTraits();
        }

        private void DEXModUpDown_ValueChanged(object sender, EventArgs e)
        {
            processStatisticsAndChosenTraits();
        }

        private void CONModUpDown_ValueChanged(object sender, EventArgs e)
        {
            processStatisticsAndChosenTraits();
        }

        private void INTModUpDown_ValueChanged(object sender, EventArgs e)
        {
            processStatisticsAndChosenTraits();
        }

        private void WISModUpDown_ValueChanged(object sender, EventArgs e)
        {
            processStatisticsAndChosenTraits();
        }

        private void CHAModUpDown_ValueChanged(object sender, EventArgs e)
        {
            processStatisticsAndChosenTraits();
        }


        //EVENTS TO SELECT THE TEXT OF THE GUI ELEMENTS ON ENTER FOCUS

        private void STRModUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, 100);
        }

        private void DEXModUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, 100);
        }

        private void CONModUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, 100);
        }

        private void INTModUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, 100);
        }

        private void WISModUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, 100);
        }

        private void CHAModUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, 100);
        }

        private void effectiveHPUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, 100);
        }

        private void effectiveACUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, 100);
        }

        private void effectiveDPRUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, 100);
        }

        private void pcLevelUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, 100);
        }

        private void namingBox_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void chosenMonTraitBox_Leave(object sender, EventArgs e)
        {
            chosenMonTraitBox.SelectedIndex=-1;
        }

        private void monsterTraitBox_Leave(object sender, EventArgs e)
        {
            monsterTraitBox.SelectedIndex = -1;
        }
    }
}
