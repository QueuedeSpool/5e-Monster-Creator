using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eMonsterCreator
{
    class CRStats
    {
        //This class has no reason to ever change once instantiated, and changing it would cause problems.
        //Therefore this class has no reasonable method to ever change. It is immutable.
        //Its member variables are private and its only member methods are the constructor and getters.

        private String myCR;
        private int myProficiencyBonus, myAC, myMaxHP, myAttackBonus, myMaxDPR, mySaveDC;

        public CRStats(String _CR, int _proficiencyBonus, int _AC, int _maxHP, int _attackBonus, int _maxDPR, int _saveDC)
        {
            myCR = _CR;
            myProficiencyBonus = _proficiencyBonus;
            myAC = _AC;
            myMaxHP = _maxHP;
            myAttackBonus = _attackBonus;
            myMaxDPR = _maxDPR;
            mySaveDC = _saveDC;
        }

        public String CR
        {
            get { return myCR; }
        }

        public int proficiencyBonus
        {
            get { return myProficiencyBonus; }
        }

        public int AC
        {
            get { return myAC; }
        }

        public int maxHP
        {
            get { return myMaxHP; }
        }

        public int attackBonus
        {
            get { return myAttackBonus; }
        }

        public int maxDPR
        {
            get { return myMaxDPR; }
        }

        public int saveDC
        {
            get { return mySaveDC; }
        }
    }
}
