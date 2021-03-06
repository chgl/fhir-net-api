/* 
 * Copyright (c) 2016, Firely (info@fire.ly) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/FirelyTeam/fhir-net-api/master/LICENSE
 */

using System;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Specification.Navigation;

namespace Hl7.Fhir.Validation
{

    internal static class ElementDefinitionNavigatorExtensions
    {
        public static string GetFhirPathConstraint(this ElementDefinition.ConstraintComponent cc)
        {
            // This was required for 3.0.0, but was rectified in the 3.0.1 technical update
            //if (cc.Key == "ele-1")
            //    return "(children().count() > id.count()) | hasValue()";
            return cc.Expression;
        }


        public static bool IsPrimitiveValueConstraint(this ElementDefinition ed)
        {
            //TODO: There is something smarter for this in STU3
            var path = ed.Path;

            return path.EndsWith(".value") && ed.Type.All(t => t.Code == null);
        }

        internal static bool IsResourcePlaceholder(this ElementDefinition ed)
        {
            if (ed.Type == null) return false;
            return ed.Type.Any(t => t.Code == "Resource" || t.Code == "DomainResource");
        }

        public static string ConstraintDescription(this ElementDefinition.ConstraintComponent cc)
        {
            var desc = cc.Key;

            if (cc.Human != null)
                desc += " \"" + cc.Human + "\"";

            return desc;
        }     
    }

}