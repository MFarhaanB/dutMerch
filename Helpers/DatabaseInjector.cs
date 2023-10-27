using BookStore.Models;
using BookStore.Models.Appointments;
using BookStore.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Helpers
{
    public class DatabaseInjector
    {
        public static void DBInjector()
        {
            //using (ApplicationDbContext dbContext = new ApplicationDbContext())
            //{
            //    foreach (String maker in Manufactures())
            //    {
            //        if (dbContext.Manufactures.FirstOrDefault(a => a.Name.Equals(maker)) == null)
            //        {
            //            dbContext.Manufactures.Add(new Manufacture { Name = maker, Id = Guid.NewGuid(), UrlLogo = "" });
            //        }
            //    }
            //    dbContext.SaveChanges();

            //    foreach (String model in VModels())
            //    {
            //        if (dbContext.VehicleModels.FirstOrDefault(a => a.Name.Equals(model)) == null)
            //        {
            //            var rr = model.Split(' ')[0];
            //            var maker = dbContext.Manufactures.FirstOrDefault(a => a.Name.Contains(rr));
            //            dbContext.VehicleModels.Add(new VehicleModel { Name = model, Id = Guid.NewGuid(), ManufactureKey = maker.Id.ToString().ToLower() });
            //        }
            //    }

            //    dbContext.SaveChanges();
            //    foreach (var type in vTypes())
            //    {
            //        String key = type.Key;
            //        List<string> values = type.Value;
            //        VehicleModel vehicleModel = dbContext.VehicleModels.FirstOrDefault(a => a.Name == key);
            //        if (vehicleModel != null)
            //        {
            //            foreach (string value in values)
            //            {
            //                if (dbContext.VehicleTypes.FirstOrDefault(a => a.Name == value) == null)
            //                {
            //                    dbContext.VehicleTypes.Add(new VehicleType
            //                    {
            //                        Name = value,
            //                        Id = Guid.NewGuid(),
            //                        VehicleModelKey = vehicleModel.Id.ToString().ToLower(),
            //                        ManufactureKey = vehicleModel.ManufactureKey
            //                    });
            //                }
            //            }
            //        }

            //    }

            //    foreach (var chkiem in GetInspectionChecklist())
            //    {
            //        int i = chkiem.Value.Count - 1;
            //        while (i >= 0)
            //        {
            //            String itemname = chkiem.Value[i];
            //            if (dbContext.CheckListItems.FirstOrDefault(a => a.Group == chkiem.Key && a.Name == itemname) == null)
            //            {
            //                CheckListItem checkListItem = new CheckListItem { Name = chkiem.Value[i], Group = chkiem.Key, Description = chkiem.Value[i] };
            //                dbContext.CheckListItems.Add(checkListItem);
            //            }
            //            i--;
            //        }
            //    }



            //    List<Status> statuses = new List<Status>();
            //    statuses.Add(new Status { Name = "Approved", Key = "STS_Approved" });
            //    statuses.Add(new Status { Name = "Declined", Key = "STS_Declined" });
            //    statuses.Add(new Status { Name = "Pending", Key = "STS_Pending" });
            //    statuses.Add(new Status { Name = "In Awaiting Review", Key = "c_enquiry_pending" });
            //    statuses.Add(new Status { Name = "In Awaiting Appointment", Key = "a_awaiting_appointment" });
            //    statuses.Add(new Status { Name = "In Awaiting Repair Acceptance", Key = "a_await_repair_acceptance" });
            //    statuses.Add(new Status { Name = "In Awaiting Auto Repairs", Key = "a_await_repair_by_m" });
            //    statuses.Add(new Status { Name = "In Awaiting Auto Checkout: No Repairs", Key = "c_no_repair_checkout" });
            //    statuses.Add(new Status { Name = "Auto Mobile Checked Out and/or Collected", Key = "c_auto_checkedout_or_colleced" });
            //    statuses.Add(new Status { Name = "c_awaiting_checkout", Key = "c_awaiting_checkout" });

            //    foreach (var status in statuses)
            //    {
            //        if (dbContext.Statuses.FirstOrDefault(a => a.Key == status.Key) == null)
            //        {
            //            status.IsActive = true;
            //            status.Description = status.Name;
            //            dbContext.Statuses.Add(status);
            //        }
            //    }

            //    dbContext.SaveChanges();

            //}
        }

        static Dictionary<string, List<string>> GetInspectionChecklist()
        {
            return new Dictionary<string, List<string>>
{
    { "Brake System", new List<string>
        {
            "Brake pads and shoes",
            "Brake rotors and drums",
            "Brake fluid level and condition",
            "Brake lines and hoses"
        }
    },
    { "Suspension and Steering", new List<string>
        {
            "Shocks and struts",
            "Control arms and bushings",
            "Tie rods and steering linkage",
            "Wheel alignment"
        }
    },
    { "Tires", new List<string>
        {
            "Tread depth and tire condition",
            "Tire pressure",
            "Wheel balance"
        }
    },
    { "Exhaust System", new List<string>
        {
            "Exhaust pipes and muffler",
            "Catalytic converter",
            "Exhaust leaks"
        }
    },
    { "Engine and Fluids", new List<string>
        {
            "Engine oil level and condition",
            "Coolant level and condition",
            "Transmission fluid level and condition",
            "Power steering fluid level",
            "Washer fluid level"
        }
    },
    { "Electrical System", new List<string>
        {
            "Battery condition and voltage",
            "Lights (headlights, taillights, brake lights, turn signals)",
            "Horn functionality",
            "Wiring and connectors"
        }
    },
    { "Safety Equipment", new List<string>
        {
            "Seat belts",
            "Airbags and supplemental restraint systems",
            "Windshield wipers and washers"
        }
    },
    { "Emissions Control", new List<string>
        {
            "Emission control system components",
            "Onboard diagnostics (OBD) system check"
        }
    },
    { "Fluid Leaks", new List<string>
        {
            "Checking for any leaks (oil, coolant, transmission fluid, etc.)"
        }
    },
    { "Undercarriage", new List<string>
        {
            "Inspecting for rust, corrosion, and damage"
        }
    },
    { "Exterior", new List<string>
        {
            "Mirrors and glass (cracks, visibility)",
            "Body damage or rust"
        }
    },
    { "Interior", new List<string>
        {
            "Dashboard warning lights",
            "Controls and gauges",
            "Interior lights"
        }
    },
    { "License and Registration", new List<string>
        {
            "Valid license plates and registration stickers"
        }
    }
};

        }

        static List<string> Manufactures()
        {
            return new List<string>
            {
            "Toyota",
            "Volkswagen Group",
            "General Motors",
            "Ford Motor Company",
            "Honda",
            "BMW Group",
            "Nissan",
            "Hyundai Motor Group",
            "Fiat Chrysler Automobiles",
            "Daimler AG",
            "Volvo Group",
            "Subaru",
            "Mazda",
            "Tesla, Inc.",
            "Mitsubishi Motors"
            };
        }

        static List<string> VModels()
        {
            return new List<string>
            {
           // Toyota models
        "Toyota GR86",
        "Toyota Corolla",
        "Toyota Camry",
        "Toyota RAV4",
        "Toyota Highlander",
        "Toyota Tacoma",

        // Honda models
        "Honda Accord",
        "Honda Civic",
        "Honda CR-V",
        "Honda Pilot",
        "Honda Fit",
        "Honda Odyssey",

        // Ford models
        "Ford Mustang",
        "Ford F-150",
        "Ford Explorer",
        "Ford Escape",
        "Ford Focus",
        "Ford Edge",

        // Volkswagen models
        "Volkswagen Golf",
        "Volkswagen Passat",
        "Volkswagen Tiguan",
        "Volkswagen Jetta",
        "Volkswagen Atlas",
        "Volkswagen Beetle",

        // General Motors (Chevrolet) models
        "Chevrolet Silverado",
        "Chevrolet Malibu",
        "Chevrolet Equinox",
        "Chevrolet Impala",
        "Chevrolet Camaro",
        "Chevrolet Traverse",

        // General Motors (GMC) models
        "GMC Sierra",
        "GMC Acadia",
        "GMC Terrain",
        "GMC Yukon",
        "GMC Canyon",
        "GMC Savana",

        // BMW models
        "BMW 3 Series",
        "BMW 5 Series",
        "BMW X3",
        "BMW X5",
        "BMW 7 Series",
        "BMW i8",

        // Nissan models
        "Nissan Altima",
        "Nissan Maxima",
        "Nissan Rogue",
        "Nissan Murano",
        "Nissan Pathfinder",
        "Nissan Sentra",

        // Hyundai models
        "Hyundai Elantra",
        "Hyundai Sonata",
        "Hyundai Tucson",
        "Hyundai Santa Fe",
        "Hyundai Kona",
        "Hyundai Palisade",

        // Fiat Chrysler (Dodge) models
        "Dodge Charger",
        "Dodge Challenger",
        "Dodge Durango",
        "Dodge Journey",
        "Dodge Grand Caravan",
        "Dodge Viper",

        // Daimler AG (Mercedes-Benz) models
        "Mercedes-Benz C-Class",
        "Mercedes-Benz E-Class",
        "Mercedes-Benz GLE",
        "Mercedes-Benz GLC",
        "Mercedes-Benz A-Class",
        "Mercedes-Benz S-Class",

        // Volvo Group models (Volvo)
        "Volvo XC60",
        "Volvo XC90",
        "Volvo S60",
        "Volvo V60",
        "Volvo XC40",
        "Volvo S90"
            // Add more models here if needed
            };
        }

        static Dictionary<string, List<string>> vTypes()
        {

            return new Dictionary<string, List<string>>
        {
             // Toyota model types
        { "Toyota Corolla", new List<string> { "Sedan", "Hatchback" } },
        { "Toyota Camry", new List<string> { "Sedan" } },
        { "Toyota RAV4", new List<string> { "SUV", "Crossover" } },
        { "Toyota Highlander", new List<string> { "SUV" } },
        { "Toyota Tacoma", new List<string> { "Pickup Truck" } },
        { "Toyota GR86", new List<string> { "Coupe" } },

        // Honda model types
        { "Honda Accord", new List<string> { "Sedan" } },
        { "Honda Civic", new List<string> { "Sedan", "Hatchback" } },
        { "Honda CR-V", new List<string> { "SUV" } },
        { "Honda Pilot", new List<string> { "SUV" } },
        { "Honda Fit", new List<string> { "Hatchback" } },
        { "Honda Odyssey", new List<string> { "Minivan" } },

        // Ford model types
        { "Ford Mustang", new List<string> { "Sports Car" } },
        { "Ford F-150", new List<string> { "Pickup Truck" } },
        { "Ford Explorer", new List<string> { "SUV" } },
        { "Ford Escape", new List<string> { "SUV" } },
        { "Ford Focus", new List<string> { "Sedan", "Hatchback" } },
        { "Ford Edge", new List<string> { "SUV" } },

        // Volkswagen model types
        { "Volkswagen Golf", new List<string> { "Hatchback" } },
        { "Volkswagen Passat", new List<string> { "Sedan" } },
        { "Volkswagen Tiguan", new List<string> { "SUV" } },
        { "Volkswagen Jetta", new List<string> { "Sedan" } },
        { "Volkswagen Atlas", new List<string> { "SUV" } },
        { "Volkswagen Beetle", new List<string> { "Hatchback" } },

        // General Motors (Chevrolet) model types
        { "Chevrolet Silverado", new List<string> { "Pickup Truck" } },
        { "Chevrolet Malibu", new List<string> { "Sedan" } },
        { "Chevrolet Equinox", new List<string> { "SUV" } },
        { "Chevrolet Impala", new List<string> { "Sedan" } },
        { "Chevrolet Camaro", new List<string> { "Sports Car" } },
        { "Chevrolet Traverse", new List<string> { "SUV" } },

        // General Motors (GMC) model types
        { "GMC Sierra", new List<string> { "Pickup Truck" } },
        { "GMC Acadia", new List<string> { "SUV" } },
        { "GMC Terrain", new List<string> { "SUV" } },
        { "GMC Yukon", new List<string> { "SUV" } },
        { "GMC Canyon", new List<string> { "Pickup Truck" } },
        { "GMC Savana", new List<string> { "Van" } },

        // BMW model types
        { "BMW 3 Series", new List<string> { "Sedan" } },
        { "BMW 5 Series", new List<string> { "Sedan" } },
        { "BMW X3", new List<string> { "SUV" } },
        { "BMW X5", new List<string> { "SUV" } },
        { "BMW 7 Series", new List<string> { "Sedan" } },
        { "BMW i8", new List<string> { "Sports Car" } },

        // Nissan model types
        { "Nissan Altima", new List<string> { "Sedan" } },
        { "Nissan Maxima", new List<string> { "Sedan" } },
        { "Nissan Rogue", new List<string> { "SUV" } },
        { "Nissan Murano", new List<string> { "SUV" } },
        { "Nissan Pathfinder", new List<string> { "SUV" } },
        { "Nissan Sentra", new List<string> { "Sedan" } },
        
        // Hyundai model types
        { "Hyundai Elantra", new List<string> { "Sedan" } },
        { "Hyundai Sonata", new List<string> { "Sedan" } },
        { "Hyundai Tucson", new List<string> { "SUV" } },
        { "Hyundai Santa Fe", new List<string> { "SUV" } },
        { "Hyundai Kona", new List<string> { "SUV" } },
        { "Hyundai Palisade", new List<string> { "SUV" } },
        
        // Fiat Chrysler (Dodge) model types
        { "Dodge Charger", new List<string> { "Sedan" } },
        { "Dodge Challenger", new List<string> { "Sports Car" } },
        { "Dodge Durango", new List<string> { "SUV" } },
        { "Dodge Journey", new List<string> { "SUV" } },
        { "Dodge Grand Caravan", new List<string> { "Minivan" } },
        { "Dodge Viper", new List<string> { "Sports Car" } },

        // Daimler AG (Mercedes-Benz) model types
        { "Mercedes-Benz C-Class", new List<string> { "Sedan" } },
        { "Mercedes-Benz E-Class", new List<string> { "Sedan" } },
        { "Mercedes-Benz GLE", new List<string> { "SUV" } },
        { "Mercedes-Benz GLC", new List<string> { "SUV" } },
        { "Mercedes-Benz A-Class", new List<string> { "Hatchback" } },
        { "Mercedes-Benz S-Class", new List<string> { "Sedan" } },

        // Volvo Group models (Volvo)
        { "Volvo XC60", new List<string> { "SUV" } },
        { "Volvo XC90", new List<string> { "SUV" } },
        { "Volvo S60", new List<string> { "Sedan" } },
        { "Volvo V60", new List<string> { "Wagon" } },
        { "Volvo XC40", new List<string> { "SUV" } },
        { "Volvo S90", new List<string> { "Sedan" } }
            // Add more models and types here if needed
        };
        }
    }
}