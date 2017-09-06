using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eMonsterCreator
{
    class MonsterTrait
    {

        private String myTraitName;
        private String myTraitDefinition;

        public MonsterTrait(String _traitName, String _traitDefinition)
        {
            myTraitName = _traitName;
            myTraitDefinition = _traitDefinition;
        }

        public String traitName
        {
            get { return myTraitName; }
        }

        public String traitDefinition
        {
            get { return myTraitDefinition; }
        }

    }
}
