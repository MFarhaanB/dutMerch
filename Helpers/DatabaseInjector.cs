using BookStore.Models;
using BookStore.Models.Appointments;
using BookStore.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Helpers
{
    public class DatabaseInjector
    {
        public static void DBInjector()
        {
            using (ApplicationDbContext  dbContext = new ApplicationDbContext()) 
            {
                foreach (String maker in Manufactures())
                {
                    if (dbContext.Manufactures.FirstOrDefault(a => a.Name.Equals(maker)) == null)
                    {
                        dbContext.Manufactures.Add(new Manufacture { Name = maker, Id = Guid.NewGuid(), UrlLogo = "" });
                    }
                }
                dbContext.SaveChanges();

                foreach (String model in VModels())
                {
                    if (dbContext.VehicleModels.FirstOrDefault(a => a.Name.Equals(model)) == null)
                    {
                        var rr = model.Split(' ')[0];
                        var maker = dbContext.Manufactures.FirstOrDefault(a => a.Name.Contains(rr));
                        dbContext.VehicleModels.Add(new VehicleModel { Name = model, Id = Guid.NewGuid(), ManufactureKey = maker.Id.ToString().ToLower() });
                    }
                }
                    
                dbContext.SaveChanges();
                foreach (var type in vTypes())
                {
                    String key = type.Key;
                    List<string> values = type.Value;
                    VehicleModel vehicleModel = dbContext.VehicleModels.FirstOrDefault(a => a.Name == key);
                    if (vehicleModel != null)
                    {
                        foreach(string value in values)
                        {
                            if(dbContext.VehicleTypes.FirstOrDefault(a => a.Name == value) == null)
                            {
                                dbContext.VehicleTypes.Add(new VehicleType
                                {
                                    Name = value,
                                    Id = Guid.NewGuid(),
                                    VehicleModelKey = vehicleModel.Id.ToString().ToLower(),
                                    ManufactureKey = vehicleModel.ManufactureKey
                                });
                            }
                        }
                    }
                  
                }

                foreach(var chkiem in GetInspectionChecklist()) 
                { 
                    int i = chkiem.Value.Count - 1;
                    while (i >= 0)
                    {
                        String itemname = chkiem.Value[i];
                        if (dbContext.CheckListItems.FirstOrDefault(a => a.Group == chkiem.Key && a.Name == itemname) == null)
                        {
                            CheckListItem checkListItem = new CheckListItem { Name = chkiem.Value[i], Group = chkiem.Key, Description = chkiem.Value[i] };
                            dbContext.CheckListItems.Add(checkListItem);
                        }
                        i--;
                    }
                }



                List<Status> statuses = new List<Status>();
                statuses.Add(new Status { Name = "Approved", Key = "STS_Approved"});
                statuses.Add(new Status { Name = "Declined", Key = "STS_Declined" });
                statuses.Add(new Status { Name = "Pending", Key = "STS_Pending" });
                statuses.Add(new Status { Name = "In Awaiting Review", Key = "c_enquiry_pending" });
                statuses.Add(new Status { Name = "In Awaiting Appointment", Key = "a_awaiting_appointment" });
                statuses.Add(new Status { Name = "In Awaiting Repair Acceptance", Key = "a_await_repair_acceptance" });
                statuses.Add(new Status { Name = "In Awaiting Auto Repairs", Key = "a_await_repair_by_m" });
                statuses.Add(new Status { Name = "In Awaiting Auto Checkout: No Repairs", Key = "c_no_repair_checkout" });
                statuses.Add(new Status { Name = "Auto Mobile Checked Out and/or Collected", Key = "c_auto_checkedout_or_colleced" });
                statuses.Add(new Status { Name = "c_awaiting_checkout", Key = "c_awaiting_checkout" });

                foreach(var status in statuses)
                {
                    if(dbContext.Statuses.FirstOrDefault(a=>a.Key == status.Key) == null)
                    {
                        status.IsActive = true;
                        status.Description = status.Name;
                        dbContext.Statuses.Add(status);
                    }
                }

                dbContext.SaveChanges();

            }
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
            "Toyota GR86",
            "Toyota Corolla",
            "Toyota Camry",
            "Toyota RAV4",
            "Toyota Highlander",
            "Toyota Tacoma",
            "Toyota Prius",
            "Toyota Sienna",
            "Toyota 4Runner",
            "Toyota Tundra",
            "Toyota Land Cruiser",
            "Toyota Sequoia",
            "Toyota C-HR",
            "Toyota Venza",
            "Toyota Yaris",
            "Toyota Avalon",
            "Toyota Supra",
            "Toyota Prius Prime",
            "Toyota Prius c",
            "Toyota Prius v",
            "Toyota Mirai"
            // Add more models here if needed
            };
        }

        static Dictionary<string, List<string>> vTypes()
        {

            return new Dictionary<string, List<string>>
        {
            { "Toyota Corolla", new List<string> { "Sedan", "Hatchback" } },
            { "Toyota Camry", new List<string> { "Sedan" } },
            { "Toyota RAV4", new List<string> { "SUV", "Crossover" } },
            { "Toyota Highlander", new List<string> { "SUV" } },
            { "Toyota Tacoma", new List<string> { "Pickup Truck" } },
            { "Toyota Prius", new List<string> { "Hybrid" } },
            { "Toyota Sienna", new List<string> { "Minivan" } },
            { "Toyota 4Runner", new List<string> { "SUV" } },
            { "Toyota Tundra", new List<string> { "Pickup Truck" } },
            { "Toyota Land Cruiser", new List<string> { "SUV" } },
            { "Toyota Sequoia", new List<string> { "SUV" } },
            { "Toyota C-HR", new List<string> { "Crossover" } },
            { "Toyota Venza", new List<string> { "Crossover" } },
            { "Toyota Yaris", new List<string> { "Sedan", "Hatchback" } },
            { "Toyota Avalon", new List<string> { "Sedan" } },
            { "Toyota Supra", new List<string> { "Sports Car" } },
            { "Toyota Prius Prime", new List<string> { "Plug-in Hybrid" } },
            { "Toyota Prius c", new List<string> { "Hybrid" } },
            { "Toyota Prius v", new List<string> { "Hybrid" } },
            { "Toyota GR86", new List<string> { "Coupe" } },
            { "Toyota Mirai", new List<string> { "Fuel Cell Vehicle (FCV)" } }
            // Add more models and types here if needed
        };
        }
    }
}