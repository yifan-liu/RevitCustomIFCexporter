using Autodesk.Revit.DB;
using Autodesk.Revit.DB.IFC;
using Revit.IFC.Common.Enums;
using Revit.IFC.Common.Utility;
using System;
using System.Collections.Generic;


namespace Revit.IFC.Export.Utility
{
    /// <summary>
    /// Provides static methods for extracting material thermal properties
    /// </summary>
    class MaterialThermalPropertiesUtil
    {

        /// <summary>
        /// Fpr each material, extract thermal properties needed for energy analysis
        /// </summary>
        /// <param name="file">the ifc file</param>
        /// <param name="materialID">The material id of that material.</param>
        /// <param name="materialHandle">The material handle.</param>
        /// <returns>True if operation succeeded.</returns>
        public static bool CreateMaterialThermalProperties(IFCFile file, ElementId materialID, IFCAnyHandle materialHandle)
        {
            try
            {
                //get the required thermal properties
                Document doc = ExporterCacheManager.Document;
                Material mat = doc.GetElement(materialID) as Material;
                ElementId thermalAseetID = mat.ThermalAssetId;
                PropertySetElement pse = doc.GetElement(thermalAseetID) as PropertySetElement;
                ThermalAsset ta = pse.GetThermalAsset();

                double densityValue = ta.Density;
                double specificHeatValue = ta.SpecificHeat;
                double thermalConductivityValue = ta.ThermalConductivity;

                //PSU: create corresponding ifchandles
                //create  and set an ifcsinglevalue for each thermal property
                IFCAnyHandle densitySingleValue = IFCAnyHandleUtil.CreateInstance(file, IFCEntityType.IfcPropertySingleValue);
                IFCAnyHandle specificHeatSingleValue = IFCAnyHandleUtil.CreateInstance(file, IFCEntityType.IfcPropertySingleValue);
                IFCAnyHandle thermalConductivitySingleValue = IFCAnyHandleUtil.CreateInstance(file, IFCEntityType.IfcPropertySingleValue);
                IFCAnyHandle roughnessSingleValue = IFCAnyHandleUtil.CreateInstance(file, IFCEntityType.IfcPropertySingleValue);

                
                //add roughness. NOTE: roughness has no unit
                int defaultRoughness = 3;
                IFCAnyHandleUtil.SetAttribute(roughnessSingleValue, "Name", "Roughness");
                IFCAnyHandleUtil.SetAttribute(roughnessSingleValue, "NominalValue", defaultRoughness);

                IFCData densityEntry = IFCData.CreateDoubleOfType(densityValue, "IfcMassDensityMeasure");
                IFCAnyHandleUtil.SetAttribute(densitySingleValue, "Name", "Density");
                IFCAnyHandleUtil.SetAttribute(densitySingleValue, "NominalValue", densityEntry);

                IFCData specificHeatEntry = IFCData.CreateDoubleOfType(specificHeatValue, "IfcSpecificHeatCapacityMeasure");
                IFCAnyHandleUtil.SetAttribute(specificHeatSingleValue, "Name", "Specific Heat");
                IFCAnyHandleUtil.SetAttribute(specificHeatSingleValue, "NominalValue", specificHeatEntry);

                IFCData thermalConductivityEntry = IFCData.CreateDoubleOfType(thermalConductivityValue, "IfcThermalConductivityMeasure");
                IFCAnyHandleUtil.SetAttribute(thermalConductivitySingleValue, "Name", "Thermal Conductivity");
                IFCAnyHandleUtil.SetAttribute(thermalConductivitySingleValue, "NominalValue", thermalConductivityEntry);

                //create and set an extendedMaterialProperties to connect a material to its thermal properties
                IFCAnyHandle ifcExtendedMaterialProperties = IFCAnyHandleUtil.CreateInstance(file, IFCEntityType.IfcExtendedMaterialProperties);
                List<IFCAnyHandle> listOfProperties = new List<IFCAnyHandle>();
                listOfProperties.Add(densitySingleValue);
                listOfProperties.Add(specificHeatSingleValue);
                listOfProperties.Add(thermalConductivitySingleValue);

                IFCAnyHandleUtil.SetAttribute(ifcExtendedMaterialProperties, "Material", materialHandle);
                IFCAnyHandleUtil.SetAttribute(ifcExtendedMaterialProperties, "ExtendedProperties", listOfProperties);
                IFCAnyHandleUtil.SetAttribute(ifcExtendedMaterialProperties, "Name", "Thermal_Properties");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
