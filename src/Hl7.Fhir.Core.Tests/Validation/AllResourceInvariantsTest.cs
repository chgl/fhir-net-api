﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hl7.Fhir.Model;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace Hl7.Fhir.FhirPath
{
    [TestClass]
    public class AllResourceInvariantsTest
    {
        [TestMethod]
        public void TestAllResourceInvariants()
        {
            string failedExpressions = null;
            foreach (var item in ModelInfo.SupportedResources)
            {
                for (int n = 0; n < 10; n++)
                {
                    Type rt = ModelInfo.GetTypeForFhirType(item);
                    Resource dr = (Resource)Activator.CreateInstance(rt);
                    dr.AddDefaultConstraints();
                    if (dr.InvariantConstraints == null || dr.InvariantConstraints.Count == 0)
                        continue;

                    Debug.WriteLine(String.Format("Testing Validation: {0} ({1} rules)", item, (dr.InvariantConstraints != null ? dr.InvariantConstraints.Count : 0)));

#if NET40
                    var results = dr.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(dr, null, null));
#else
                    var results = dr.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(dr));
#endif
                    foreach (var result in results)
                    {
                        if (result.ErrorMessage.Contains("FATAL"))
                            failedExpressions += result.ErrorMessage + "\r\n";

                        Console.WriteLine(result.ErrorMessage);
                    }
                    //if (results.Count() > 0)

                    Debug.WriteLine("");
                }
            }
            Assert.IsNull(failedExpressions);
        }
    }
}
