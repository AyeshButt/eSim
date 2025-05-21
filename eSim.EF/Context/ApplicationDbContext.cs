using eSim.EF.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<SystemClaims> SystemClaims { get; set; }
        public DbSet<SideMenu> SideMenu { get; set; }
        public DbSet<OTPVerification> OTPVerification { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<ClientSettings> ClientSettings { get; set; }
        public DbSet<GlobalSetting> GlobalSetting { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketAttachments> TicketAttachments { get; set; }
        public DbSet<TicketActivities> TicketActivities { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<Subscribers> Subscribers { get; set; }
        public DbSet<AspNetUsersType> AspNetUsersType { get; set; }

        public DbSet<Countries> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Countries>(entity =>
            {
                entity.ToTable("Countries");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.CountryName)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.Iso3)
                    .HasMaxLength(3)
                    .IsRequired();

                entity.Property(e => e.Iso2)
                    .HasMaxLength(2)
                    .IsRequired();

                // Seeding data
                entity.HasData(new Countries { Id = 1, CountryName = "Afghanistan", Iso3 = "AFG", Iso2 = "AF" },
new Countries { Id = 2, CountryName = "Aland Islands", Iso3 = "ALA", Iso2 = "AX" },
new Countries { Id = 3, CountryName = "Albania", Iso3 = "ALB", Iso2 = "AL" },
new Countries { Id = 4, CountryName = "Algeria", Iso3 = "DZA", Iso2 = "DZ" },
new Countries { Id = 5, CountryName = "American Samoa", Iso3 = "ASM", Iso2 = "AS" },
new Countries { Id = 6, CountryName = "Andorra", Iso3 = "AND", Iso2 = "AD" },
new Countries { Id = 7, CountryName = "Angola", Iso3 = "AGO", Iso2 = "AO" },
new Countries { Id = 8, CountryName = "Anguilla", Iso3 = "AIA", Iso2 = "AI" },
new Countries { Id = 9, CountryName = "Antarctica", Iso3 = "ATA", Iso2 = "AQ" },
new Countries { Id = 10, CountryName = "Antigua And Barbuda", Iso3 = "ATG", Iso2 = "AG" },
new Countries { Id = 11, CountryName = "Argentina", Iso3 = "ARG", Iso2 = "AR" },
new Countries { Id = 12, CountryName = "Armenia", Iso3 = "ARM", Iso2 = "AM" },
new Countries { Id = 13, CountryName = "Aruba", Iso3 = "ABW", Iso2 = "AW" },
new Countries { Id = 14, CountryName = "Australia", Iso3 = "AUS", Iso2 = "AU" },
new Countries { Id = 15, CountryName = "Austria", Iso3 = "AUT", Iso2 = "AT" },
new Countries { Id = 16, CountryName = "Azerbaijan", Iso3 = "AZE", Iso2 = "AZ" },
new Countries { Id = 17, CountryName = "Bahamas The", Iso3 = "BHS", Iso2 = "BS" },
new Countries { Id = 18, CountryName = "Bahrain", Iso3 = "BHR", Iso2 = "BH" },
new Countries { Id = 19, CountryName = "Bangladesh", Iso3 = "BGD", Iso2 = "BD" },
new Countries { Id = 20, CountryName = "Barbados", Iso3 = "BRB", Iso2 = "BB" },
new Countries { Id = 21, CountryName = "Belarus", Iso3 = "BLR", Iso2 = "BY" },
new Countries { Id = 22, CountryName = "Belgium", Iso3 = "BEL", Iso2 = "BE" },
new Countries { Id = 23, CountryName = "Belize", Iso3 = "BLZ", Iso2 = "BZ" },
new Countries { Id = 24, CountryName = "Benin", Iso3 = "BEN", Iso2 = "BJ" },
new Countries { Id = 25, CountryName = "Bermuda", Iso3 = "BMU", Iso2 = "BM" },
new Countries { Id = 26, CountryName = "Bhutan", Iso3 = "BTN", Iso2 = "BT" },
new Countries { Id = 27, CountryName = "Bolivia", Iso3 = "BOL", Iso2 = "BO" },
new Countries { Id = 28, CountryName = "Bosnia and Herzegovina", Iso3 = "BIH", Iso2 = "BA" },
new Countries { Id = 29, CountryName = "Botswana", Iso3 = "BWA", Iso2 = "BW" },
new Countries { Id = 30, CountryName = "Bouvet Island", Iso3 = "BVT", Iso2 = "BV" },
new Countries { Id = 31, CountryName = "Brazil", Iso3 = "BRA", Iso2 = "BR" },
new Countries { Id = 32, CountryName = "British Indian Ocean Territory", Iso3 = "IOT", Iso2 = "IO" },
new Countries { Id = 33, CountryName = "Brunei", Iso3 = "BRN", Iso2 = "BN" },
new Countries { Id = 34, CountryName = "Bulgaria", Iso3 = "BGR", Iso2 = "BG" },
new Countries { Id = 35, CountryName = "Burkina Faso", Iso3 = "BFA", Iso2 = "BF" },
new Countries { Id = 36, CountryName = "Burundi", Iso3 = "BDI", Iso2 = "BI" },
new Countries { Id = 37, CountryName = "Cambodia", Iso3 = "KHM", Iso2 = "KH" },
new Countries { Id = 38, CountryName = "Cameroon", Iso3 = "CMR", Iso2 = "CM" },
new Countries { Id = 39, CountryName = "Canada", Iso3 = "CAN", Iso2 = "CA" },
new Countries { Id = 40, CountryName = "Cape Verde", Iso3 = "CPV", Iso2 = "CV" },
new Countries { Id = 41, CountryName = "Cayman Islands", Iso3 = "CYM", Iso2 = "KY" },
new Countries { Id = 42, CountryName = "Central African Republic", Iso3 = "CAF", Iso2 = "CF" },
new Countries { Id = 43, CountryName = "Chad", Iso3 = "TCD", Iso2 = "TD" },
new Countries { Id = 44, CountryName = "Chile", Iso3 = "CHL", Iso2 = "CL" },
new Countries { Id = 45, CountryName = "China", Iso3 = "CHN", Iso2 = "CN" },
new Countries { Id = 46, CountryName = "Christmas Island", Iso3 = "CXR", Iso2 = "CX" },
new Countries { Id = 47, CountryName = "Cocos (Keeling) Islands", Iso3 = "CCK", Iso2 = "CC" },
new Countries { Id = 48, CountryName = "Colombia", Iso3 = "COL", Iso2 = "CO" },
new Countries { Id = 49, CountryName = "Comoros", Iso3 = "COM", Iso2 = "KM" },
new Countries { Id = 50, CountryName = "Congo, The Republic of", Iso3 = "COG", Iso2 = "CG" },
new Countries { Id = 51, CountryName = "Congo The Democratic Republic Of The", Iso3 = "COD", Iso2 = "CD" },
new Countries { Id = 52, CountryName = "Cook Islands", Iso3 = "COK", Iso2 = "CK" },
new Countries { Id = 53, CountryName = "Costa Rica", Iso3 = "CRI", Iso2 = "CR" },
new Countries { Id = 54, CountryName = "Cote D'Ivoire (Ivory Coast)", Iso3 = "CIV", Iso2 = "CI" },
new Countries { Id = 55, CountryName = "Croatia (Hrvatska)", Iso3 = "HRV", Iso2 = "HR" },
new Countries { Id = 56, CountryName = "Cuba", Iso3 = "CUB", Iso2 = "CU" },
new Countries { Id = 57, CountryName = "Cyprus, Republic of", Iso3 = "CYP", Iso2 = "CY" },
new Countries { Id = 58, CountryName = "Czech Republic", Iso3 = "CZE", Iso2 = "CZ" },
new Countries { Id = 59, CountryName = "Denmark", Iso3 = "DNK", Iso2 = "DK" },
new Countries { Id = 60, CountryName = "Djibouti", Iso3 = "DJI", Iso2 = "DJ" },
new Countries { Id = 61, CountryName = "Dominica", Iso3 = "DMA", Iso2 = "DM" },
new Countries { Id = 62, CountryName = "Dominican Republic", Iso3 = "DOM", Iso2 = "DO" },
new Countries { Id = 63, CountryName = "East Timor", Iso3 = "TLS", Iso2 = "TL" },
new Countries { Id = 64, CountryName = "Ecuador", Iso3 = "ECU", Iso2 = "EC" },
new Countries { Id = 65, CountryName = "Egypt", Iso3 = "EGY", Iso2 = "EG" },
new Countries { Id = 66, CountryName = "El Salvador", Iso3 = "SLV", Iso2 = "SV" },
new Countries { Id = 67, CountryName = "Equatorial Guinea", Iso3 = "GNQ", Iso2 = "GQ" },
new Countries { Id = 68, CountryName = "Eritrea", Iso3 = "ERI", Iso2 = "ER" },
new Countries { Id = 69, CountryName = "Estonia", Iso3 = "EST", Iso2 = "EE" },
new Countries { Id = 70, CountryName = "Ethiopia", Iso3 = "ETH", Iso2 = "ET" },
new Countries { Id = 71, CountryName = "Falkland Islands", Iso3 = "FLK", Iso2 = "FK" },
new Countries { Id = 72, CountryName = "Faroe Islands", Iso3 = "FRO", Iso2 = "FO" },
new Countries { Id = 73, CountryName = "Fiji Islands", Iso3 = "FJI", Iso2 = "FJ" },
new Countries { Id = 74, CountryName = "Finland", Iso3 = "FIN", Iso2 = "FI" },
new Countries { Id = 75, CountryName = "France", Iso3 = "FRA", Iso2 = "FR" },
new Countries { Id = 76, CountryName = "French Guiana", Iso3 = "GUF", Iso2 = "GF" },
new Countries { Id = 77, CountryName = "French Polynesia", Iso3 = "PYF", Iso2 = "PF" },
new Countries { Id = 78, CountryName = "French Southern Territories", Iso3 = "ATF", Iso2 = "TF" },
new Countries { Id = 79, CountryName = "Gabon", Iso3 = "GAB", Iso2 = "GA" },
new Countries { Id = 80, CountryName = "Gambia The", Iso3 = "GMB", Iso2 = "GM" },
new Countries { Id = 81, CountryName = "Georgia", Iso3 = "GEO", Iso2 = "GE" },
new Countries { Id = 82, CountryName = "Germany", Iso3 = "DEU", Iso2 = "DE" },
new Countries { Id = 83, CountryName = "Ghana", Iso3 = "GHA", Iso2 = "GH" },
new Countries { Id = 84, CountryName = "Gibraltar", Iso3 = "GIB", Iso2 = "GI" },
new Countries { Id = 85, CountryName = "Greece", Iso3 = "GRC", Iso2 = "GR" },
new Countries { Id = 86, CountryName = "Greenland", Iso3 = "GRL", Iso2 = "GL" },
new Countries { Id = 87, CountryName = "Grenada", Iso3 = "GRD", Iso2 = "GD" },
new Countries { Id = 88, CountryName = "Guadeloupe", Iso3 = "GLP", Iso2 = "GP" },
new Countries { Id = 89, CountryName = "Guam", Iso3 = "GUM", Iso2 = "GU" },
new Countries { Id = 90, CountryName = "Guatemala", Iso3 = "GTM", Iso2 = "GT" },
new Countries { Id = 91, CountryName = "Guernsey and Alderney", Iso3 = "GGY", Iso2 = "GG" },
new Countries { Id = 92, CountryName = "Guinea", Iso3 = "GIN", Iso2 = "GN" },
new Countries { Id = 93, CountryName = "Guinea-Bissau", Iso3 = "GNB", Iso2 = "GW" },
new Countries { Id = 94, CountryName = "Guyana", Iso3 = "GUY", Iso2 = "GY" },
new Countries { Id = 95, CountryName = "Haiti", Iso3 = "HTI", Iso2 = "HT" },
new Countries { Id = 96, CountryName = "Heard Island and McDonald Islands", Iso3 = "HMD", Iso2 = "HM" },
new Countries { Id = 97, CountryName = "Honduras", Iso3 = "HND", Iso2 = "HN" },
new Countries { Id = 98, CountryName = "Hong Kong S.A.R.", Iso3 = "HKG", Iso2 = "HK" },
new Countries { Id = 99, CountryName = "Hungary", Iso3 = "HUN", Iso2 = "HU" },
new Countries { Id = 100, CountryName = "Iceland", Iso3 = "ISL", Iso2 = "IS" },
new Countries { Id = 101, CountryName = "India", Iso3 = "IND", Iso2 = "IN" },
new Countries { Id = 102, CountryName = "Indonesia", Iso3 = "IDN", Iso2 = "ID" },
new Countries { Id = 103, CountryName = "Iran", Iso3 = "IRN", Iso2 = "IR" },
new Countries { Id = 104, CountryName = "Iraq", Iso3 = "IRQ", Iso2 = "IQ" },
new Countries { Id = 105, CountryName = "Ireland", Iso3 = "IRL", Iso2 = "IE" },
new Countries { Id = 106, CountryName = "Israel", Iso3 = "ISR", Iso2 = "IL" },
new Countries { Id = 107, CountryName = "Italy", Iso3 = "ITA", Iso2 = "IT" },
new Countries { Id = 108, CountryName = "Jamaica", Iso3 = "JAM", Iso2 = "JM" },
new Countries { Id = 109, CountryName = "Japan", Iso3 = "JPN", Iso2 = "JP" },
new Countries { Id = 110, CountryName = "Jersey", Iso3 = "JEY", Iso2 = "JE" },
new Countries { Id = 111, CountryName = "Jordan", Iso3 = "JOR", Iso2 = "JO" },
new Countries { Id = 112, CountryName = "Kazakhstan", Iso3 = "KAZ", Iso2 = "KZ" },
new Countries { Id = 113, CountryName = "Kenya", Iso3 = "KEN", Iso2 = "KE" },
new Countries { Id = 114, CountryName = "Kiribati", Iso3 = "KIR", Iso2 = "KI" },
new Countries { Id = 115, CountryName = "South Korea (Republic of)", Iso3 = "PRK", Iso2 = "KP" },
new Countries { Id = 116, CountryName = "North Korea (Democratic People's Republic of)", Iso3 = "KOR", Iso2 = "KR" },
new Countries { Id = 117, CountryName = "Kuwait", Iso3 = "KWT", Iso2 = "KW" },
new Countries { Id = 118, CountryName = "Kyrgyzstan", Iso3 = "KGZ", Iso2 = "KG" },
new Countries { Id = 119, CountryName = "Laos", Iso3 = "LAO", Iso2 = "LA" },
new Countries { Id = 120, CountryName = "Latvia", Iso3 = "LVA", Iso2 = "LV" },
new Countries { Id = 121, CountryName = "Lebanon", Iso3 = "LBN", Iso2 = "LB" },
new Countries { Id = 122, CountryName = "Lesotho", Iso3 = "LSO", Iso2 = "LS" },
new Countries { Id = 123, CountryName = "Liberia", Iso3 = "LBR", Iso2 = "LR" },
new Countries { Id = 124, CountryName = "Libya", Iso3 = "LBY", Iso2 = "LY" },
new Countries { Id = 125, CountryName = "Liechtenstein", Iso3 = "LIE", Iso2 = "LI" },
new Countries { Id = 126, CountryName = "Lithuania", Iso3 = "LTU", Iso2 = "LT" },
new Countries { Id = 127, CountryName = "Luxembourg", Iso3 = "LUX", Iso2 = "LU" },
new Countries { Id = 128, CountryName = "Macau S.A.R.", Iso3 = "MAC", Iso2 = "MO" },
new Countries { Id = 129, CountryName = "North Macedonia", Iso3 = "MKD", Iso2 = "MK" },
new Countries { Id = 130, CountryName = "Madagascar", Iso3 = "MDG", Iso2 = "MG" },
new Countries { Id = 131, CountryName = "Malawi", Iso3 = "MWI", Iso2 = "MW" },
new Countries { Id = 132, CountryName = "Malaysia", Iso3 = "MYS", Iso2 = "MY" },
new Countries { Id = 133, CountryName = "Maldives", Iso3 = "MDV", Iso2 = "MV" },
new Countries { Id = 134, CountryName = "Mali", Iso3 = "MLI", Iso2 = "ML" },
new Countries { Id = 135, CountryName = "Malta", Iso3 = "MLT", Iso2 = "MT" },
new Countries { Id = 136, CountryName = "Man (Isle of)", Iso3 = "IMN", Iso2 = "IM" },
new Countries { Id = 137, CountryName = "Marshall Islands", Iso3 = "MHL", Iso2 = "MH" },
new Countries { Id = 138, CountryName = "Martinique", Iso3 = "MTQ", Iso2 = "MQ" },
new Countries { Id = 139, CountryName = "Mauritania", Iso3 = "MRT", Iso2 = "MR" },
new Countries { Id = 140, CountryName = "Mauritius", Iso3 = "MUS", Iso2 = "MU" },
new Countries { Id = 141, CountryName = "Mayotte", Iso3 = "MYT", Iso2 = "YT" },
new Countries { Id = 142, CountryName = "Mexico", Iso3 = "MEX", Iso2 = "MX" },
new Countries { Id = 143, CountryName = "Micronesia", Iso3 = "FSM", Iso2 = "FM" },
new Countries { Id = 144, CountryName = "Moldova", Iso3 = "MDA", Iso2 = "MD" },
new Countries { Id = 145, CountryName = "Monaco", Iso3 = "MCO", Iso2 = "MC" },
new Countries { Id = 146, CountryName = "Mongolia", Iso3 = "MNG", Iso2 = "MN" },
new Countries { Id = 147, CountryName = "Montenegro", Iso3 = "MNE", Iso2 = "ME" },
new Countries { Id = 148, CountryName = "Montserrat", Iso3 = "MSR", Iso2 = "MS" },
new Countries { Id = 149, CountryName = "Morocco", Iso3 = "MAR", Iso2 = "MA" },
new Countries { Id = 150, CountryName = "Mozambique", Iso3 = "MOZ", Iso2 = "MZ" },
new Countries { Id = 151, CountryName = "Myanmar", Iso3 = "MMR", Iso2 = "MM" },
new Countries { Id = 152, CountryName = "Namibia", Iso3 = "NAM", Iso2 = "NA" },
new Countries { Id = 153, CountryName = "Nauru", Iso3 = "NRU", Iso2 = "NR" },
new Countries { Id = 154, CountryName = "Nepal", Iso3 = "NPL", Iso2 = "NP" },
new Countries { Id = 155, CountryName = "Bonaire, Sint Eustatius and Saba", Iso3 = "BES", Iso2 = "BQ" },
new Countries { Id = 156, CountryName = "Netherlands The", Iso3 = "NLD", Iso2 = "NL" },
new Countries { Id = 157, CountryName = "New Caledonia", Iso3 = "NCL", Iso2 = "NC" },
new Countries { Id = 158, CountryName = "New Zealand", Iso3 = "NZL", Iso2 = "NZ" },
new Countries { Id = 159, CountryName = "Nicaragua", Iso3 = "NIC", Iso2 = "NI" },
new Countries { Id = 160, CountryName = "Niger", Iso3 = "NER", Iso2 = "NE" },
new Countries { Id = 161, CountryName = "Nigeria", Iso3 = "NGA", Iso2 = "NG" },
new Countries { Id = 162, CountryName = "Niue", Iso3 = "NIU", Iso2 = "NU" },
new Countries { Id = 163, CountryName = "Norfolk Island", Iso3 = "NFK", Iso2 = "NF" },
new Countries { Id = 164, CountryName = "Northern Mariana Islands", Iso3 = "MNP", Iso2 = "MP" },
new Countries { Id = 165, CountryName = "Norway", Iso3 = "NOR", Iso2 = "NO" },
new Countries { Id = 166, CountryName = "Oman", Iso3 = "OMN", Iso2 = "OM" },
new Countries { Id = 167, CountryName = "Pakistan", Iso3 = "PAK", Iso2 = "PK" },
new Countries { Id = 168, CountryName = "Palau", Iso3 = "PLW", Iso2 = "PW" },
new Countries { Id = 169, CountryName = "Palestine", Iso3 = "PSE", Iso2 = "PS" },
new Countries { Id = 170, CountryName = "Panama", Iso3 = "PAN", Iso2 = "PA" },
new Countries { Id = 171, CountryName = "Papua New Guinea", Iso3 = "PNG", Iso2 = "PG" },
new Countries { Id = 172, CountryName = "Paraguay", Iso3 = "PRY", Iso2 = "PY" },
new Countries { Id = 173, CountryName = "Peru", Iso3 = "PER", Iso2 = "PE" },
new Countries { Id = 174, CountryName = "Philippines", Iso3 = "PHL", Iso2 = "PH" },
new Countries { Id = 175, CountryName = "Pitcairn Island", Iso3 = "PCN", Iso2 = "PN" },
new Countries { Id = 176, CountryName = "Poland", Iso3 = "POL", Iso2 = "PL" },
new Countries { Id = 177, CountryName = "Portugal", Iso3 = "PRT", Iso2 = "PT" },
new Countries { Id = 178, CountryName = "Puerto Rico", Iso3 = "PRI", Iso2 = "PR" },
new Countries { Id = 179, CountryName = "Qatar", Iso3 = "QAT", Iso2 = "QA" },
new Countries { Id = 180, CountryName = "Reunion", Iso3 = "REU", Iso2 = "RE" },
new Countries { Id = 181, CountryName = "Romania", Iso3 = "ROU", Iso2 = "RO" },
new Countries { Id = 182, CountryName = "Russia", Iso3 = "RUS", Iso2 = "RU" },
new Countries { Id = 183, CountryName = "Rwanda", Iso3 = "RWA", Iso2 = "RW" },
new Countries { Id = 184, CountryName = "Saint Helena", Iso3 = "SHN", Iso2 = "SH" },
new Countries { Id = 185, CountryName = "Saint Kitts And Nevis", Iso3 = "KNA", Iso2 = "KN" },
new Countries { Id = 186, CountryName = "Saint Lucia", Iso3 = "LCA", Iso2 = "LC" },
new Countries { Id = 187, CountryName = "Saint Pierre and Miquelon", Iso3 = "SPM", Iso2 = "PM" },
new Countries { Id = 188, CountryName = "Saint Vincent And The Grenadines", Iso3 = "VCT", Iso2 = "VC" },
new Countries { Id = 189, CountryName = "Saint-Barthelemy", Iso3 = "BLM", Iso2 = "BL" },
new Countries { Id = 190, CountryName = "Saint-Martin (French part)", Iso3 = "MAF", Iso2 = "MF" },
new Countries { Id = 191, CountryName = "Samoa", Iso3 = "WSM", Iso2 = "WS" },
new Countries { Id = 192, CountryName = "San Marino", Iso3 = "SMR", Iso2 = "SM" },
new Countries { Id = 193, CountryName = "Sao Tome and Principe", Iso3 = "STP", Iso2 = "ST" },
new Countries { Id = 194, CountryName = "Saudi Arabia", Iso3 = "SAU", Iso2 = "SA" },
new Countries { Id = 195, CountryName = "Senegal", Iso3 = "SEN", Iso2 = "SN" },
new Countries { Id = 196, CountryName = "Serbia", Iso3 = "SRB", Iso2 = "RS" },
new Countries { Id = 197, CountryName = "Seychelles", Iso3 = "SYC", Iso2 = "SC" },
new Countries { Id = 198, CountryName = "Sierra Leone", Iso3 = "SLE", Iso2 = "SL" },
new Countries { Id = 199, CountryName = "Singapore", Iso3 = "SGP", Iso2 = "SG" },
new Countries { Id = 200, CountryName = "Slovakia", Iso3 = "SVK", Iso2 = "SK" },
new Countries { Id = 201, CountryName = "Slovenia", Iso3 = "SVN", Iso2 = "SI" },
new Countries { Id = 202, CountryName = "Solomon Islands", Iso3 = "SLB", Iso2 = "SB" },
new Countries { Id = 203, CountryName = "Somalia", Iso3 = "SOM", Iso2 = "SO" },
new Countries { Id = 204, CountryName = "South Africa", Iso3 = "ZAF", Iso2 = "ZA" },
new Countries { Id = 205, CountryName = "South Georgia", Iso3 = "SGS", Iso2 = "GS" },
new Countries { Id = 206, CountryName = "South Sudan", Iso3 = "SSD", Iso2 = "SS" },
new Countries { Id = 207, CountryName = "Spain", Iso3 = "ESP", Iso2 = "ES" },
new Countries { Id = 208, CountryName = "Sri Lanka", Iso3 = "LKA", Iso2 = "LK" },
new Countries { Id = 209, CountryName = "Sudan", Iso3 = "SDN", Iso2 = "SD" },
new Countries { Id = 210, CountryName = "Suriname", Iso3 = "SUR", Iso2 = "SR" },
new Countries { Id = 211, CountryName = "Svalbard And Jan Mayen Islands", Iso3 = "SJM", Iso2 = "SJ" },
new Countries { Id = 212, CountryName = "Eswatini", Iso3 = "SWZ", Iso2 = "SZ" },
new Countries { Id = 213, CountryName = "Sweden", Iso3 = "SWE", Iso2 = "SE" },
new Countries { Id = 214, CountryName = "Switzerland", Iso3 = "CHE", Iso2 = "CH" },
new Countries { Id = 215, CountryName = "Syria", Iso3 = "SYR", Iso2 = "SY" },
new Countries { Id = 216, CountryName = "Taiwan", Iso3 = "TWN", Iso2 = "TW" },
new Countries { Id = 217, CountryName = "Tajikistan", Iso3 = "TJK", Iso2 = "TJ" },
new Countries { Id = 218, CountryName = "Tanzania", Iso3 = "TZA", Iso2 = "TZ" },
new Countries { Id = 219, CountryName = "Thailand", Iso3 = "THA", Iso2 = "TH" },
new Countries { Id = 220, CountryName = "Togo", Iso3 = "TGO", Iso2 = "TG" },
new Countries { Id = 221, CountryName = "Tokelau", Iso3 = "TKL", Iso2 = "TK" },
new Countries { Id = 222, CountryName = "Tonga", Iso3 = "TON", Iso2 = "TO" },
new Countries { Id = 223, CountryName = "Trinidad And Tobago", Iso3 = "TTO", Iso2 = "TT" },
new Countries { Id = 224, CountryName = "Tunisia", Iso3 = "TUN", Iso2 = "TN" },
new Countries { Id = 225, CountryName = "Turkey", Iso3 = "TUR", Iso2 = "TR" },
new Countries { Id = 226, CountryName = "Turkmenistan", Iso3 = "TKM", Iso2 = "TM" },
new Countries { Id = 227, CountryName = "Turks And Caicos Islands", Iso3 = "TCA", Iso2 = "TC" },
new Countries { Id = 228, CountryName = "Tuvalu", Iso3 = "TUV", Iso2 = "TV" },
new Countries { Id = 229, CountryName = "Uganda", Iso3 = "UGA", Iso2 = "UG" },
new Countries { Id = 230, CountryName = "Ukraine", Iso3 = "UKR", Iso2 = "UA" },
new Countries { Id = 231, CountryName = "United Arab Emirates", Iso3 = "ARE", Iso2 = "AE" },
new Countries { Id = 232, CountryName = "United Kingdom", Iso3 = "GBR", Iso2 = "GB" },
new Countries { Id = 233, CountryName = "United States of America", Iso3 = "USA", Iso2 = "US" },
new Countries { Id = 234, CountryName = "United States Minor Outlying Islands", Iso3 = "UMI", Iso2 = "UM" },
new Countries { Id = 235, CountryName = "Uruguay", Iso3 = "URY", Iso2 = "UY" },
new Countries { Id = 236, CountryName = "Uzbekistan", Iso3 = "UZB", Iso2 = "UZ" },
new Countries { Id = 237, CountryName = "Vanuatu", Iso3 = "VUT", Iso2 = "VU" },
new Countries { Id = 238, CountryName = "Vatican City State (Holy See)", Iso3 = "VAT", Iso2 = "VA" },
new Countries { Id = 239, CountryName = "Venezuela", Iso3 = "VEN", Iso2 = "VE" },
new Countries { Id = 240, CountryName = "Vietnam", Iso3 = "VNM", Iso2 = "VN" },
new Countries { Id = 241, CountryName = "Virgin Islands (British)", Iso3 = "VGB", Iso2 = "VG" },
new Countries { Id = 242, CountryName = "Virgin Islands (US)", Iso3 = "VIR", Iso2 = "VI" },
new Countries { Id = 243, CountryName = "Wallis And Futuna Islands", Iso3 = "WLF", Iso2 = "WF" },
new Countries { Id = 244, CountryName = "Western Sahara", Iso3 = "ESH", Iso2 = "EH" },
new Countries { Id = 245, CountryName = "Yemen", Iso3 = "YEM", Iso2 = "YE" },
new Countries { Id = 246, CountryName = "Zambia", Iso3 = "ZMB", Iso2 = "ZM" },
new Countries { Id = 247, CountryName = "Zimbabwe", Iso3 = "ZWE", Iso2 = "ZW" },
new Countries { Id = 248, CountryName = "Kosovo", Iso3 = "XKX", Iso2 = "XK" },
new Countries { Id = 249, CountryName = "Curaçao", Iso3 = "CUW", Iso2 = "CW" },
new Countries { Id = 250, CountryName = "Sint Maarten (Dutch part)", Iso3 = "SXM", Iso2 = "SX" },
new Countries { Id = 251, CountryName = "Cyprus, Turkish Republic of Northern", Iso3 = "CYP", Iso2 = "CY" },
new Countries { Id = 252, CountryName = "South Georgia and the South Sandwich Islands", Iso3 = "SGS", Iso2 = "GS" }
);
            });



            modelBuilder.Entity<ClientSettings>(entity =>
            {
                entity.HasOne(e => e.Client)
                      .WithMany()
                      .HasForeignKey(e => e.ClientId);
            });

            modelBuilder.Entity<TicketType>().HasData(
               new TicketType { Id = 1, Type = "Bundle" },
               new TicketType { Id = 2, Type = "Payment" }
           );
            modelBuilder.Entity<TicketStatus>().HasData(
               new TicketStatus { Id = 1, Status = "Open" },
               new TicketStatus { Id = 2, Status = "Close" },
               new TicketStatus { Id = 3, Status = "In-progress" },
               new TicketStatus { Id = 4, Status = "Waiting for reply" }

           );
            modelBuilder.Entity<TicketAttachmentType>().HasData(
               new TicketAttachmentType { Id = 1, AttachmentType = "Internal" },
               new TicketAttachmentType { Id = 2, AttachmentType = "External" }
           );
            modelBuilder.Entity<TicketCommentType>().HasData(
               new TicketCommentType { Id = 1, CommentType = "Customer" },
               new TicketCommentType { Id = 2, CommentType = "Admin" }

           );
                        modelBuilder.Entity<AspNetUsersType>().HasData(
               new AspNetUsersType { Id = 1, Type = "Developer" },
               new AspNetUsersType { Id = 2, Type = "Superadmin" },
               new AspNetUsersType { Id = 3, Type = "Subadmin" },
               new AspNetUsersType { Id = 4, Type = "Client" },
               new AspNetUsersType { Id = 5, Type = "Subclient" }

);
        }
    }
}
