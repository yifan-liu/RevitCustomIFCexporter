using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.IFC;

namespace Revit.IFC.Export.Exporter.PropertySet.Calculators
{
    class MaterialThermalPropertiesCalculator : PropertyCalculator
    {

        public enum ParamType {Density, SpecificHeat, ThermalConductivity};

        private double density;
        private double specificHeat;
        private double thermalConductivity;

        public double Density { get { return density; } }
        public double SpecificHeat { get { return specificHeat; } }
        public double ThermalConductivity { get { return thermalConductivity; } }

        static MaterialThermalPropertiesCalculator s_instance;

        public static MaterialThermalPropertiesCalculator instance
        {
            get { return s_instance; }
        }

        public override bool Calculate(ExporterIFC exporterIFC, IFCExtrusionCreationData extrusionCreationData, Element element, ElementType elementType)
        {
            ICollection<ElementId> matIDs = element.GetMaterialIds(false);
            Document doc = element.Document;
            foreach (ElementId id in matIDs)
            {
                //Material mat = 
            }

            return false;
        }


        //this calculator calculates multiple parameters
        public override bool CalculatesMultipleParameters
        {
            get
            {
                return true;
            }
        }


        //get the value of the property by param type
        public double GetDoubleValue(ParamType type)
        {
            switch (type)
            {
                case ParamType.Density:
                    return Density;
                case ParamType.SpecificHeat:
                    return SpecificHeat;
                case ParamType.ThermalConductivity:
                    return thermalConductivity;
                default:
                    throw new Exception("invalid param type!\n");
            }

        }


        //public override List<double> Ge

    }
}
