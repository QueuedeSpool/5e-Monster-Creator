using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eMonsterCreator
{
    class MonsterTraitTable
    {
        private List<MonsterTrait> table;

        public MonsterTraitTable()
        {
            table = new List<MonsterTrait>();

            //table.Add(new MonsterTrait("No Armor", "aacadd~dex_mod"));
            //table.Add(new MonsterTrait("Light/Medium/Heavy Armor", "aacadd~AC_Bonus+IIF(dex_mod<=Maximum_Dex_Bonus_From_Armor,dex_mod,Maximum_Dex_Bonus_From_Armor) vars~AC_Bonus,Maximum_Dex_Bonus_From_Armor"));
            table.Add(new MonsterTrait("Regeneration", "ehpadd~3*(Healing_Per_Round) vars~Healing_Per_Round"));
            table.Add(new MonsterTrait("Fiendish Blessing", "aacadd~cha_mod"));
            table.Add(new MonsterTrait("Bard's Revenge", "cr1-4:edpradd~absurdity cr5-10:edpradd~absurdity*2 cr11-16:edpradd~absurdity*3 cr17-30:edpradd~absurdity*4 vars~absurdity"));
            table.Add(new MonsterTrait("Frightful Presence", "pcl<=10:ehpmult~1.25 pcl>10:ehpmult~.75"));
            table.Add(new MonsterTrait("Deathly Countenance", "ehpadd~-500"));
        }

        //This method can return null.
        //Always check for null.
        public String[] monsterTraitNames
        {
            get
            {
                int length = table.Count;

                if (length == 0)
                {
                    return null;
                }

                String[] names = new String[length];

                for (int i = 0; i < length; i++)
                {
                    names[i] = table[i].traitName;
                }

                return names;
            }
        }

        //This method can return null.
        //Always check for null.
        public MonsterTrait getMonsterTraitByIterator(int i)
        {
            if (table.Count > i)
            {
                return table.ElementAt(i);
            }

            return null;
        }

        //This method can return null.
        //Always check for null.
        public MonsterTrait getMonsterTraitByTraitName(String _propertyName)
        {
            for (int i = 0; i < table.Count; i++)
            {
                if (table[i].traitName.Equals(_propertyName, StringComparison.Ordinal))
                {
                    return table[i];
                }
            }
            return null;
        }
    }
}
