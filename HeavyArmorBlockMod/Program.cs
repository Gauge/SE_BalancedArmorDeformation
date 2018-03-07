using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace HeavyArmorBlockMod
{
    class Program
    {

        public static string fileDirectory = @"E:\Games\SteamGames\steamapps\common\SpaceEngineers\Content\Data\"; // directory to the SE's Content/Data/.sbc's
        public static string ouputDirectory = @"."; // path to write the new sbc.

        public static float Large_Light_Ratio = 0.05f;
        public static float Large_Heavy_Ratio = 0.05f;
        public static float Small_Light_Ratio = 0.05f;
        public static float Small_Heavy_Ratio = 0.05f;
        public static float General_Damage_Multiplier = 0.9f;

        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Parsing SE CubeBlocks...");
                string path = Path.Combine(fileDirectory, "CubeBlocks.sbc");
                XDocument document = XDocument.Load(path);
                XDocument resultsDocument = XDocument.Load(path);

                ApplyChanges(document, resultsDocument);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not fined CubeBlocks.sbc at {fileDirectory} or file failed to parse");
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        public static void ApplyChanges(XDocument document, XDocument results)
        {
            IEnumerable<XElement> definitions = document.Root.XPathSelectElements("//Definition");

            Console.WriteLine("Preping new CubeBlocks...");
            XElement resultsCubeElement = results.Root.Element("CubeBlocks");
            resultsCubeElement.RemoveNodes();
            results.Root.Element("BlockPositions").Remove();

            XElement LLA = new XElement("DeformationRatio", Large_Light_Ratio);
            XElement LHA = new XElement("DeformationRatio", Large_Heavy_Ratio);
            XElement SLA = new XElement("DeformationRatio", Small_Light_Ratio);
            XElement SHA = new XElement("DeformationRatio", Small_Heavy_Ratio);
            XElement GDR = new XElement("GeneralDamageMultiplier", General_Damage_Multiplier);

            foreach (XElement definition in definitions)
            {
                string displayName = definition.Element("DisplayName")?.Value;
                string typeId = definition.Element("Id").Element("TypeId")?.Value;
                string subtype = definition.Element("Id").Element("SubtypeId")?.Value;
                XElement deformationRatio = definition.Element("DeformationRatio");
                XElement generalDamageReduction = definition.Element("GeneralDamageMultiplier");

                if (typeId == "CubeBlock")
                {
                    if (subtype.Contains("Large") && subtype.Contains("Armor") && !subtype.Contains("Heavy"))
                    {
                        Console.WriteLine($"Applying change to {displayName}");
                        if (deformationRatio == null)
                        {
                            definition.Add(LLA);
                        }
                        else
                        {
                            deformationRatio.Value = Large_Light_Ratio.ToString();
                        }

                        resultsCubeElement.Add(definition);
                    }
                    else if (subtype.Contains("Large") && subtype.Contains("Armor") && subtype.Contains("Heavy"))
                    {
                        Console.WriteLine($"Applying change to {displayName}");
                        if (deformationRatio == null)
                        {
                            definition.Add(LHA);
                        }
                        else
                        {
                            deformationRatio.Value = Large_Heavy_Ratio.ToString();
                        }

                        if (generalDamageReduction == null)
                        {
                            definition.Add(GDR);
                        }
                        else
                        {
                            generalDamageReduction.Value = General_Damage_Multiplier.ToString();
                        }

                        resultsCubeElement.Add(definition);
                    }
                    else if (subtype.Contains("Small") && subtype.Contains("Armor") && !subtype.Contains("Heavy"))
                    {
                        Console.WriteLine($"Applying change to {displayName}");
                        if (deformationRatio == null)
                        {
                            definition.Add(SLA);
                        }
                        else
                        {
                            deformationRatio.Value = Small_Light_Ratio.ToString();
                        }

                        resultsCubeElement.Add(definition);
                    }
                    else if (subtype.Contains("Small") && subtype.Contains("Armor") && subtype.Contains("Heavy"))
                    {
                        Console.WriteLine($"Applying change to {displayName}");
                        if (deformationRatio == null)
                        {
                            definition.Add(SHA);
                        }
                        else
                        {
                            deformationRatio.Value = Small_Heavy_Ratio.ToString();
                        }

                        if (generalDamageReduction == null)
                        {
                            definition.Add(GDR);
                        }
                        else
                        {
                            generalDamageReduction.Value = General_Damage_Multiplier.ToString();
                        }

                        resultsCubeElement.Add(definition);
                    }
                }
            }

            results.Save(Path.Combine(ouputDirectory, "CubeBlocks.sbc"));
        }
    }
}
