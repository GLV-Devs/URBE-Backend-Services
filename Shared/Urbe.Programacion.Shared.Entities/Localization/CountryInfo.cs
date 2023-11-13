using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Urbe.Programacion.Shared.Entities.Localization;

public sealed class CountryInfo
{
    public CountryInfo(string name, string officialStateName, string sovereignty, string alpha2Code, string alpha3Code, int numericCodeInt, string internetccTL)
    {
        Name = name;
        NameESP = name;
        OfficialStateName = officialStateName;
        Sovereignty = sovereignty;
        Alpha2Code = alpha2Code;
        Alpha3Code = alpha3Code;
        NumericCodeInt = numericCodeInt;
        InternetccTL = internetccTL;
        NumericCode = NumericCodeInt.ToString("000");
    }

    public CountryInfo(string name, string nameESP, string officialStateName, string sovereignty, string alpha2Code, string alpha3Code, int numericCodeInt, string internetccTL)
    {
        Name = name;
        NameESP = nameESP;
        OfficialStateName = officialStateName;
        Sovereignty = sovereignty;
        Alpha2Code = alpha2Code;
        Alpha3Code = alpha3Code;
        NumericCodeInt = numericCodeInt;
        InternetccTL = internetccTL;
        NumericCode = NumericCodeInt.ToString("000");
    }

    public string Name { get; }
    public string NameESP { get; }
    public string OfficialStateName { get; }
    public string Sovereignty { get; }
    public string Alpha2Code { get; }
    public string Alpha3Code { get; }
    public int NumericCodeInt { get; }
    public string InternetccTL { get; }
    public string NumericCode { get; }

    private string? str;
    public override string ToString()
        => str ??= $"{Name} ({Alpha3Code})";

    private readonly static object sync = new();

    private static Dictionary<string, CountryInfo>? byAlpha2Code;
    private static Dictionary<string, CountryInfo>? byAlpha3Code;
    private static Dictionary<string, CountryInfo>? byNumericCode;
    private static Dictionary<int, CountryInfo>? byNumericCodeInt;
    private static Dictionary<string, CountryInfo>? byInternetccTLD;
    private static CountryInfo[]? countries;

    private static Dictionary<TKey, CountryInfo> GetOrCreate<TKey>(ref Dictionary<TKey, CountryInfo>? dictionary, Func<CountryInfo, TKey> keySelector)
        where TKey : notnull
    {
        if (dictionary is null)
            lock (sync)
                if (dictionary is null)
                {
                    return dictionary = typeof(CountryInfo)
                        .GetProperties(BindingFlags.Static | BindingFlags.Public)
                        .Select(x => (CountryInfo)x.GetValue(null)!)
                        .ToDictionary(keySelector);
                }

        return dictionary;
    }

    public static IEnumerable<CountryInfo> GetCountries()
    {
        if (countries is null)
            lock (sync)
                if (countries is null)
                {
                    var props = typeof(CountryInfo).GetProperties(BindingFlags.Static | BindingFlags.Public);
                    countries = new CountryInfo[props.Length];

                    int i = 0;
                    foreach (var p in props)
                        countries[i++] = (CountryInfo)p.GetValue(null)!;
                }
        return countries;
    }

    public static CountryInfo GetByAlpha2Code(string code)
        => GetOrCreate(ref byAlpha2Code, x => x.Alpha2Code)[code];

    public static CountryInfo GetByAlpha3Code(string code)
        => GetOrCreate(ref byAlpha3Code, x => x.Alpha3Code)[code];

    public static CountryInfo GetByNumericCode(string code)
        => GetOrCreate(ref byNumericCode, x => x.NumericCode)[code];

    public static CountryInfo GetByNumericCode(int code)
        => GetOrCreate(ref byNumericCodeInt, x => x.NumericCodeInt)[code];

    public static CountryInfo GetByInternetccTLD(string domain)
        => GetOrCreate(ref byInternetccTLD, x => x.InternetccTL)[domain];

    public static bool TryGetByAlpha2Code(string code, [NotNullWhen(true)] out CountryInfo? result)
        => GetOrCreate(ref byAlpha2Code, x => x.Alpha2Code).TryGetValue(code, out result);

    public static bool TryGetByAlpha3Code(string code, [NotNullWhen(true)] out CountryInfo? result)
        => GetOrCreate(ref byAlpha3Code, x => x.Alpha3Code).TryGetValue(code, out result);

    public static bool TryGetByNumericCode(string code, [NotNullWhen(true)] out CountryInfo? result)
        => GetOrCreate(ref byNumericCode, x => x.NumericCode).TryGetValue(code, out result);

    public static bool TryGetByNumericCode(int code, [NotNullWhen(true)] out CountryInfo? result)
        => GetOrCreate(ref byNumericCodeInt, x => x.NumericCodeInt).TryGetValue(code, out result);

    public static bool TryGetByInternetccTLD(string domain, [NotNullWhen(true)] out CountryInfo? result)
        => GetOrCreate(ref byInternetccTLD, x => x.InternetccTL).TryGetValue(domain, out result);

    public static CountryInfo Afghanistan { get; } = new(
        "Afghanistan",
        "Afghanistan",
        "The Islamic Republic of Afghanistan",
        "UN member state",
        "AF",
        "AFG",
        004,
        ".af"
    );

    public static CountryInfo AlandIslands { get; } = new(
        "Ãland",
        "Ãland",
        "Ãland",
        "Finland",
        "AX",
        "ALA",
        248,
        ".ax"
    );

    public static CountryInfo Albania { get; } = new(
        "Albania",
        "Albania",
        "The Republic of Albania",
        "UN member state",
        "AL",
        "ALB",
        008,
        ".al"
    );

    public static CountryInfo Algeria { get; } = new(
        "Algeria",
        "Algeria",
        "The People's Democratic Republic of Algeria",
        "UN member state",
        "DZ",
        "DZA",
        012,
        ".dz"
    );

    public static CountryInfo AmericanSamoa { get; } = new(
        "American Samoa",
        "Samoa Americana",
        "The Territory of American Samoa",
        "United States",
        "AS",
        "ASM",
        016,
        ".as"
    );

    public static CountryInfo Andorra { get; } = new(
        "Andorra",
        "Andorra",
        "The Principality of Andorra",
        "UN member state",
        "AD",
        "AND",
        020,
        ".ad"
    );

    public static CountryInfo Angola { get; } = new(
        "Angola",
        "Angola",
        "The Republic of Angola",
        "UN member state",
        "AO",
        "AGO",
        024,
        ".ao"
    );

    public static CountryInfo Anguilla { get; } = new(
        "Anguilla",
        "Anguilla",
        "Anguilla",
        "United Kingdom",
        "AI",
        "AIA",
        660,
        ".ai"
    );

    public static CountryInfo Antarctica { get; } = new(
        "Antarctica",
        "Antártica",
        "All land and ice shelves south of the 60th parallel south",
        "Antarctic Treaty",
        "AQ",
        "ATA",
        010,
        ".aq"
    );

    public static CountryInfo AntiguaandBarbuda { get; } = new(
        "Antigua and Barbuda",
        "Antigua y Barbuda",
        "Antigua and Barbuda",
        "UN member state",
        "AG",
        "ATG",
        028,
        ".ag"
    );

    public static CountryInfo Argentina { get; } = new(
        "Argentina",
        "Argentina",
        "The Argentine Republic",
        "UN member state",
        "AR",
        "ARG",
        032,
        ".ar"
    );

    public static CountryInfo Armenia { get; } = new(
        "Armenia",
        "Armenia",
        "The Republic of Armenia",
        "UN member state",
        "AM",
        "ARM",
        051,
        ".am"
    );

    public static CountryInfo Aruba { get; } = new(
        "Aruba",
        "Aruba",
        "Aruba",
        "Netherlands",
        "AW",
        "ABW",
        533,
        ".aw"
    );

    public static CountryInfo Australia { get; } = new(
        "Australia",
        "Australia",
        "The Commonwealth of Australia",
        "UN member state",
        "AU",
        "AUS",
        036,
        ".au"
    );

    public static CountryInfo Austria { get; } = new(
        "Austria",
        "Austria",
        "The Republic of Austria",
        "UN member state",
        "AT",
        "AUT",
        040,
        ".at"
    );

    public static CountryInfo Azerbaijan { get; } = new(
        "Azerbaijan",
        "Azerbaiyán",
        "The Republic of Azerbaijan",
        "UN member state",
        "AZ",
        "AZE",
        031,
        ".az"
    );

    public static CountryInfo Bahamas { get; } = new(
        "Bahamas (the)",
        "Bahamas (las)",
        "The Commonwealth of The Bahamas",
        "UN member state",
        "BS",
        "BHS",
        044,
        ".bs"
    );

    public static CountryInfo Bahrain { get; } = new(
        "Bahrain",
        "Baréin (Reino de)",
        "The Kingdom of Bahrain",
        "UN member state",
        "BH",
        "BHR",
        048,
        ".bh"
    );

    public static CountryInfo Bangladesh { get; } = new(
        "Bangladesh",
        "Bangladesh",
        "The People's Republic of Bangladesh",
        "UN member state",
        "BD",
        "BGD",
        050,
        ".bd"
    );

    public static CountryInfo Barbados { get; } = new(
        "Barbados",
        "Barbados",
        "Barbados",
        "UN member state",
        "BB",
        "BRB",
        052,
        ".bb"
    );

    public static CountryInfo Belarus { get; } = new(
        "Belarus",
        "Bielorrusia",
        "The Republic of Belarus",
        "UN member state",
        "BY",
        "BLR",
        112,
        ".by"
    );

    public static CountryInfo Belgium { get; } = new(
        "Belgium",
        "Bélgica",
        "The Kingdom of Belgium",
        "UN member state",
        "BE",
        "BEL",
        056,
        ".be"
    );

    public static CountryInfo Belize { get; } = new(
        "Belize",
        "Belice",
        "Belize",
        "UN member state",
        "BZ",
        "BLZ",
        084,
        ".bz"
    );

    public static CountryInfo Benin { get; } = new(
        "Benin",
        "Benín",
        "The Republic of Benin",
        "UN member state",
        "BJ",
        "BEN",
        204,
        ".bj"
    );

    public static CountryInfo Bermuda { get; } = new(
        "Bermuda",
        "Bermudas",
        "Bermuda",
        "United Kingdom",
        "BM",
        "BMU",
        060,
        ".bm"
    );

    public static CountryInfo Bhutan { get; } = new(
        "Bhutan",
        "Bután",
        "The Kingdom of Bhutan",
        "UN member state",
        "BT",
        "BTN",
        064,
        ".bt"
    );

    public static CountryInfo Bolivia { get; } = new(
        "Bolivia",
        "Bolivia",
        "The Plurinational State of Bolivia",
        "UN member state",
        "BO",
        "BOL",
        068,
        ".bo"
    );

    public static CountryInfo Bonaire { get; } = new(
        "Bonaire",
        "Bonaire",
        "Bonaire, Sint Eustatius and Saba",
        "Netherlands",
        "BQ",
        "BES",
        535,
        ".bq .nl"
    );

    public static CountryInfo BosniaandHerzegovina { get; } = new(
        "Bosnia and Herzegovina",
        "Bosnia y Herzegovina",
        "Bosnia and Herzegovina",
        "UN member state",
        "BA",
        "BIH",
        070,
        ".ba"
    );

    public static CountryInfo Botswana { get; } = new(
        "Botswana",
        "Botsuana",
        "The Republic of Botswana",
        "UN member state",
        "BW",
        "BWA",
        072,
        ".bw"
    );

    public static CountryInfo BouvetIsland { get; } = new(
        "Bouvet Island",
        "Isla Bouvet",
        "Bouvet Island",
        "Norway",
        "BV",
        "BVT",
        074,
        ""
    );

    public static CountryInfo Brazil { get; } = new(
        "Brazil",
        "Brasil",
        "The Federative Republic of Brazil",
        "UN member state",
        "BR",
        "BRA",
        076,
        ".br"
    );

    public static CountryInfo BritishIndianOceanTerritory { get; } = new(
        "British Indian Ocean Territory (the)",
        "Territorio Británico del Océano Índico",
        "The British Indian Ocean Territory",
        "United Kingdom",
        "IO",
        "IOT",
        086,
        ".io"
    );

    public static CountryInfo BruneiDarussalam { get; } = new(
        "Brunei Darussalam",
        "Brunéi",
        "The Nation of Brunei, the Abode of Peace",
        "UN member state",
        "BN",
        "BRN",
        096,
        ".bn"
    );

    public static CountryInfo Bulgaria { get; } = new(
        "Bulgaria",
        "Bulgaria",
        "The Republic of Bulgaria",
        "UN member state",
        "BG",
        "BGR",
        100,
        ".bg"
    );

    public static CountryInfo BurkinaFaso { get; } = new(
        "Burkina Faso",
        "Burkina Faso",
        "Burkina Faso",
        "UN member state",
        "BF",
        "BFA",
        854,
        ".bf"
    );

    public static CountryInfo Burundi { get; } = new(
        "Burundi",
        "Burundi",
        "The Republic of Burundi",
        "UN member state",
        "BI",
        "BDI",
        108,
        ".bi"
    );

    public static CountryInfo CaboVerde { get; } = new(
        "Cabo Verde",
        "Cabo Verde",
        "The Republic of Cabo Verde",
        "UN member state",
        "CV",
        "CPV",
        132,
        ".cv"
    );

    public static CountryInfo Cambodia { get; } = new(
        "Cambodia",
        "Cambodia",
        "The Kingdom of Cambodia",
        "UN member state",
        "KH",
        "KHM",
        116,
        ".kh"
    );

    public static CountryInfo Cameroon { get; } = new(
        "Cameroon",
        "Camerún",
        "The Republic of Cameroon",
        "UN member state",
        "CM",
        "CMR",
        120,
        ".cm"
    );

    public static CountryInfo Canada { get; } = new(
        "Canada",
        "Canada",
        "Canada",
        "UN member state",
        "CA",
        "CAN",
        124,
        ".ca"
    );

    public static CountryInfo CaymanIslands { get; } = new(
        "Cayman Islands (the)",
        "Islas Caimán",
        "The Cayman Islands",
        "United Kingdom",
        "KY",
        "CYM",
        136,
        ".ky"
    );

    public static CountryInfo CentralAfricanRepublic { get; } = new(
        "Central African Republic (the)",
        "República Centroafricana",
        "The Central African Republic",
        "UN member state",
        "CF",
        "CAF",
        140,
        ".cf"
    );

    public static CountryInfo Chad { get; } = new(
        "Chad",
        "Chad",
        "The Republic of Chad",
        "UN member state",
        "TD",
        "TCD",
        148,
        ".td"
    );

    public static CountryInfo Chile { get; } = new(
        "Chile",
        "Chile",
        "The Republic of Chile",
        "UN member state",
        "CL",
        "CHL",
        152,
        ".cl"
    );

    public static CountryInfo China { get; } = new(
        "China",
        "China",
        "The People's Republic of China",
        "UN member state",
        "CN",
        "CHN",
        156,
        ".cn"
    );

    public static CountryInfo ChristmasIsland { get; } = new(
        "Christmas Island",
        "Kiritimati",
        "The Territory of Christmas Island",
        "Australia",
        "CX",
        "CXR",
        162,
        ".cx"
    );

    public static CountryInfo Cocos { get; } = new(
        "Cocos (Keeling) Islands (the)",
        "Islas Cocos",
        "The Territory of Cocos (Keeling) Islands",
        "Australia",
        "CC",
        "CCK",
        166,
        ".cc"
    );

    public static CountryInfo Colombia { get; } = new(
        "Colombia",
        "Colombia",
        "The Republic of Colombia",
        "UN member state",
        "CO",
        "COL",
        170,
        ".co"
    );

    public static CountryInfo Comoros { get; } = new(
        "Comoros (the)",
        "Comoras",
        "The Union of the Comoros",
        "UN member state",
        "KM",
        "COM",
        174,
        ".km"
    );

    public static CountryInfo DemocraticRepublicOfCongo { get; } = new(
        "Congo (the Democratic Republic of the)",
        "República Democratica del Congo",
        "The Democratic Republic of the Congo",
        "UN member state",
        "CD",
        "COD",
        180,
        ".cd"
    );

    public static CountryInfo RepublicOfCongo { get; } = new(
        "Congo (the)",
        "El Congo",
        "The Republic of the Congo",
        "UN member state",
        "CG",
        "COG",
        178,
        ".cg"
    );

    public static CountryInfo CookIslands { get; } = new(
        "Cook Islands (the)",
        "Islas Cook",
        "The Cook Islands",
        "New Zealand",
        "CK",
        "COK",
        184,
        ".ck"
    );

    public static CountryInfo CostaRica { get; } = new(
        "Costa Rica",
        "Costa Rica",
        "The Republic of Costa Rica",
        "UN member state",
        "CR",
        "CRI",
        188,
        ".cr"
    );

    public static CountryInfo CatedIvoire { get; } = new(
        "CÃ´te d'Ivoire",
        "Islas de Marfíl",
        "The Republic of CÃ´te d'Ivoire",
        "UN member state",
        "CI",
        "CIV",
        384,
        ".ci"
    );

    public static CountryInfo Croatia { get; } = new(
        "Croatia",
        "Croacia",
        "The Republic of Croatia",
        "UN member state",
        "HR",
        "HRV",
        191,
        ".hr"
    );

    public static CountryInfo Cuba { get; } = new(
        "Cuba",
        "Cuba",
        "The Republic of Cuba",
        "UN member state",
        "CU",
        "CUB",
        192,
        ".cu"
    );

    public static CountryInfo Curacao { get; } = new(
        "Curacao",
        "Curacao",
        "The Country of Curacao",
        "Netherlands",
        "CW",
        "CUW",
        531,
        ".cw"
    );

    public static CountryInfo Cyprus { get; } = new(
        "Cyprus",
        "Chipre",
        "The Republic of Cyprus",
        "UN member state",
        "CY",
        "CYP",
        196,
        ".cy"
    );

    public static CountryInfo Czechia { get; } = new(
        "Czechia",
        "República Checa",
        "The Czech Republic",
        "UN member state",
        "CZ",
        "CZE",
        203,
        ".cz"
    );

    public static CountryInfo Denmark { get; } = new(
        "Denmark",
        "Dinamarca",
        "The Kingdom of Denmark",
        "UN member state",
        "DK",
        "DNK",
        208,
        ".dk"
    );

    public static CountryInfo Djibouti { get; } = new(
        "Djibouti",
        "Yibuti",
        "The Republic of Djibouti",
        "UN member state",
        "DJ",
        "DJI",
        262,
        ".dj"
    );

    public static CountryInfo Dominica { get; } = new(
        "Dominica",
        "Dominica",
        "The Commonwealth of Dominica",
        "UN member state",
        "DM",
        "DMA",
        212,
        ".dm"
    );

    public static CountryInfo DominicanRepublic { get; } = new(
        "Dominican Republic (the)",
        "República Dominicana",
        "The Dominican Republic",
        "UN member state",
        "DO",
        "DOM",
        214,
        ".do"
    );

    public static CountryInfo Ecuador { get; } = new(
        "Ecuador",
        "Ecuador",
        "The Republic of Ecuador",
        "UN member state",
        "EC",
        "ECU",
        218,
        ".ec"
    );

    public static CountryInfo Egypt { get; } = new(
        "Egypt",
        "Egipto",
        "The Arab Republic of Egypt",
        "UN member state",
        "EG",
        "EGY",
        818,
        ".eg"
    );

    public static CountryInfo ElSalvador { get; } = new(
        "El Salvador",
        "El Salvador",
        "The Republic of El Salvador",
        "UN member state",
        "SV",
        "SLV",
        222,
        ".sv"
    );

    public static CountryInfo EquatorialGuinea { get; } = new(
        "Equatorial Guinea",
        "Guinea Ecuatorial",
        "The Republic of Equatorial Guinea",
        "UN member state",
        "GQ",
        "GNQ",
        226,
        ".gq"
    );

    public static CountryInfo Eritrea { get; } = new(
        "Eritrea",
        "The State of Eritrea",
        "UN member state",
        "ER",
        "ERI",
        232,
        ".er"
    );

    public static CountryInfo Estonia { get; } = new(
        "Estonia",
        "Estonia",
        "The Republic of Estonia",
        "UN member state",
        "EE",
        "EST",
        233,
        ".ee"
    );

    public static CountryInfo Eswatini { get; } = new(
        "Eswatini",
        "The Kingdom of Eswatini",
        "UN member state",
        "SZ",
        "SWZ",
        748,
        ".sz"
    );

    public static CountryInfo Ethiopia { get; } = new(
        "Ethiopia",
        "The Federal Democratic Republic of Ethiopia",
        "UN member state",
        "ET",
        "ETH",
        231,
        ".et"
    );

    public static CountryInfo FalklandIslands { get; } = new(
        "Falkland Islands (the) [Malvinas]",
        "The Falkland Islands",
        "United Kingdom",
        "FK",
        "FLK",
        238,
        ".fk"
    );

    public static CountryInfo FaroeIslands { get; } = new(
        "Faroe Islands (the)",
        "The Faroe Islands",
        "Denmark",
        "FO",
        "FRO",
        234,
        ".fo"
    );

    public static CountryInfo Fiji { get; } = new(
        "Fiji",
        "The Republic of Fiji",
        "UN member state",
        "FJ",
        "FJI",
        242,
        ".fj"
    );

    public static CountryInfo Finland { get; } = new(
        "Finland",
        "The Republic of Finland",
        "UN member state",
        "FI",
        "FIN",
        246,
        ".fi"
    );

    public static CountryInfo France { get; } = new(
        "France",
        "The French Republic",
        "UN member state",
        "FR",
        "FRA",
        250,
        ".fr"
    );

    public static CountryInfo FrenchGuiana { get; } = new(
        "French Guiana",
        "Guyane",
        "France",
        "GF",
        "GUF",
        254,
        ".gf"
    );

    public static CountryInfo FrenchPolynesia { get; } = new(
        "French Polynesia",
        "French Polynesia",
        "France",
        "PF",
        "PYF",
        258,
        ".pf"
    );

    public static CountryInfo FrenchSouthernTerritories { get; } = new(
        "French Southern Territories (the)",
        "The French Southern and Antarctic Lands",
        "France",
        "TF",
        "ATF",
        260,
        ".tf"
    );

    public static CountryInfo Gabon { get; } = new(
        "Gabon",
        "The Gabonese Republic",
        "UN member state",
        "GA",
        "GAB",
        266,
        ".ga"
    );

    public static CountryInfo Gambia { get; } = new(
        "Gambia (the)",
        "The Republic of The Gambia",
        "UN member state",
        "GM",
        "GMB",
        270,
        ".gm"
    );

    public static CountryInfo Georgia { get; } = new(
        "Georgia",
        "Georgia",
        "UN member state",
        "GE",
        "GEO",
        268,
        ".ge"
    );

    public static CountryInfo Germany { get; } = new(
        "Germany",
        "The Federal Republic of Germany",
        "UN member state",
        "DE",
        "DEU",
        276,
        ".de"
    );

    public static CountryInfo Ghana { get; } = new(
        "Ghana",
        "The Republic of Ghana",
        "UN member state",
        "GH",
        "GHA",
        288,
        ".gh"
    );

    public static CountryInfo Gibraltar { get; } = new(
        "Gibraltar",
        "Gibraltar",
        "United Kingdom",
        "GI",
        "GIB",
        292,
        ".gi"
    );

    public static CountryInfo Greece { get; } = new(
        "Greece",
        "The Hellenic Republic",
        "UN member state",
        "GR",
        "GRC",
        300,
        ".gr"
    );

    public static CountryInfo Greenland { get; } = new(
        "Greenland",
        "Kalaallit Nunaat",
        "Denmark",
        "GL",
        "GRL",
        304,
        ".gl"
    );

    public static CountryInfo Grenada { get; } = new(
        "Grenada",
        "Grenada",
        "UN member state",
        "GD",
        "GRD",
        308,
        ".gd"
    );

    public static CountryInfo Guadeloupe { get; } = new(
        "Guadeloupe",
        "Guadeloupe",
        "France",
        "GP",
        "GLP",
        312,
        ".gp"
    );

    public static CountryInfo Guam { get; } = new(
        "Guam",
        "The Territory of Guam",
        "United States",
        "GU",
        "GUM",
        316,
        ".gu"
    );

    public static CountryInfo Guatemala { get; } = new(
        "Guatemala",
        "The Republic of Guatemala",
        "UN member state",
        "GT",
        "GTM",
        320,
        ".gt"
    );

    public static CountryInfo Guernsey { get; } = new(
        "Guernsey",
        "The Bailiwick of Guernsey",
        "British Crown",
        "GG",
        "GGY",
        831,
        ".gg"
    );

    public static CountryInfo Guinea { get; } = new(
        "Guinea",
        "The Republic of Guinea",
        "UN member state",
        "GN",
        "GIN",
        324,
        ".gn"
    );

    public static CountryInfo GuineaBissau { get; } = new(
        "Guinea-Bissau",
        "The Republic of Guinea-Bissau",
        "UN member state",
        "GW",
        "GNB",
        624,
        ".gw"
    );

    public static CountryInfo Guyana { get; } = new(
        "Guyana",
        "The Co-operative Republic of Guyana",
        "UN member state",
        "GY",
        "GUY",
        328,
        ".gy"
    );

    public static CountryInfo Haiti { get; } = new(
        "Haiti",
        "The Republic of Haiti",
        "UN member state",
        "HT",
        "HTI",
        332,
        ".ht"
    );

    public static CountryInfo HeardIslandandMcDonaldIslands { get; } = new(
        "Heard Island and McDonald Islands",
        "The Territory of Heard Island and McDonald Islands",
        "Australia",
        "HM",
        "HMD",
        334,
        ".hm"
    );

    public static CountryInfo HolySee { get; } = new(
        "Holy See (the)",
        "The Holy See",
        "UN observer state",
        "VA",
        "VAT",
        336,
        ".va"
    );

    public static CountryInfo Honduras { get; } = new(
        "Honduras",
        "The Republic of Honduras",
        "UN member state",
        "HN",
        "HND",
        340,
        ".hn"
    );

    public static CountryInfo HongKong { get; } = new(
        "Hong Kong",
        "The Hong Kong Special Administrative Region of China",
        "China",
        "HK",
        "HKG",
        344,
        ".hk"
    );

    public static CountryInfo Hungary { get; } = new(
        "Hungary",
        "Hungary",
        "UN member state",
        "HU",
        "HUN",
        348,
        ".hu"
    );

    public static CountryInfo Iceland { get; } = new(
        "Iceland",
        "Iceland",
        "UN member state",
        "IS",
        "ISL",
        352,
        ".is"
    );

    public static CountryInfo India { get; } = new(
        "India",
        "The Republic of India",
        "UN member state",
        "IN",
        "IND",
        356,
        ".in"
    );

    public static CountryInfo Indonesia { get; } = new(
        "Indonesia",
        "The Republic of Indonesia",
        "UN member state",
        "ID",
        "IDN",
        360,
        ".id"
    );

    public static CountryInfo Iran { get; } = new(
        "Iran (Islamic Republic of)",
        "The Islamic Republic of Iran",
        "UN member state",
        "IR",
        "IRN",
        364,
        ".ir"
    );

    public static CountryInfo Iraq { get; } = new(
        "Iraq",
        "The Republic of Iraq",
        "UN member state",
        "IQ",
        "IRQ",
        368,
        ".iq"
    );

    public static CountryInfo Ireland { get; } = new(
        "Ireland",
        "Ireland",
        "UN member state",
        "IE",
        "IRL",
        372,
        ".ie"
    );

    public static CountryInfo IsleofMan { get; } = new(
        "Isle of Man",
        "The Isle of Man",
        "British Crown",
        "IM",
        "IMN",
        833,
        ".im"
    );

    public static CountryInfo Israel { get; } = new(
        "Israel",
        "The State of Israel",
        "UN member state",
        "IL",
        "ISR",
        376,
        ".il"
    );

    public static CountryInfo Italy { get; } = new(
        "Italy",
        "The Italian Republic",
        "UN member state",
        "IT",
        "ITA",
        380,
        ".it"
    );

    public static CountryInfo Jamaica { get; } = new(
        "Jamaica",
        "Jamaica",
        "UN member state",
        "JM",
        "JAM",
        388,
        ".jm"
    );

    public static CountryInfo Japan { get; } = new(
        "Japan",
        "Japan",
        "UN member state",
        "JP",
        "JPN",
        392,
        ".jp"
    );

    public static CountryInfo Jersey { get; } = new(
        "Jersey",
        "The Bailiwick of Jersey",
        "British Crown",
        "JE",
        "JEY",
        832,
        ".je"
    );

    public static CountryInfo Jordan { get; } = new(
        "Jordan",
        "The Hashemite Kingdom of Jordan",
        "UN member state",
        "JO",
        "JOR",
        400,
        ".jo"
    );

    public static CountryInfo Kazakhstan { get; } = new(
        "Kazakhstan",
        "The Republic of Kazakhstan",
        "UN member state",
        "KZ",
        "KAZ",
        398,
        ".kz"
    );

    public static CountryInfo Kenya { get; } = new(
        "Kenya",
        "The Republic of Kenya",
        "UN member state",
        "KE",
        "KEN",
        404,
        ".ke"
    );

    public static CountryInfo Kiribati { get; } = new(
        "Kiribati",
        "The Republic of Kiribati",
        "UN member state",
        "KI",
        "KIR",
        296,
        ".ki"
    );

    public static CountryInfo NorthKorea { get; } = new(
        "Korea (the Democratic People's Republic of)",
        "The Democratic People's Republic of Korea",
        "UN member state",
        "KP",
        "PRK",
        408,
        ".kp"
    );

    public static CountryInfo SouthKorea { get; } = new(
        "Korea (the Republic of)",
        "The Republic of Korea",
        "UN member state",
        "KR",
        "KOR",
        410,
        ".kr"
    );

    public static CountryInfo Kuwait { get; } = new(
        "Kuwait",
        "The State of Kuwait",
        "UN member state",
        "KW",
        "KWT",
        414,
        ".kw"
    );

    public static CountryInfo Kyrgyzstan { get; } = new(
        "Kyrgyzstan",
        "The Kyrgyz Republic",
        "UN member state",
        "KG",
        "KGZ",
        417,
        ".kg"
    );

    public static CountryInfo LaoPeoplesDemocraticRepublic { get; } = new(
        "Lao People's Democratic Republic (the)",
        "The Lao People's Democratic Republic",
        "UN member state",
        "LA",
        "LAO",
        418,
        ".la"
    );

    public static CountryInfo Latvia { get; } = new(
        "Latvia",
        "The Republic of Latvia",
        "UN member state",
        "LV",
        "LVA",
        428,
        ".lv"
    );

    public static CountryInfo Lebanon { get; } = new(
        "Lebanon",
        "The Lebanese Republic",
        "UN member state",
        "LB",
        "LBN",
        422,
        ".lb"
    );

    public static CountryInfo Lesotho { get; } = new(
        "Lesotho",
        "The Kingdom of Lesotho",
        "UN member state",
        "LS",
        "LSO",
        426,
        ".ls"
    );

    public static CountryInfo Liberia { get; } = new(
        "Liberia",
        "The Republic of Liberia",
        "UN member state",
        "LR",
        "LBR",
        430,
        ".lr"
    );

    public static CountryInfo Libya { get; } = new(
        "Libya",
        "The State of Libya",
        "UN member state",
        "LY",
        "LBY",
        434,
        ".ly"
    );

    public static CountryInfo Liechtenstein { get; } = new(
        "Liechtenstein",
        "The Principality of Liechtenstein",
        "UN member state",
        "LI",
        "LIE",
        438,
        ".li"
    );

    public static CountryInfo Lithuania { get; } = new(
        "Lithuania",
        "The Republic of Lithuania",
        "UN member state",
        "LT",
        "LTU",
        440,
        ".lt"
    );

    public static CountryInfo Luxembourg { get; } = new(
        "Luxembourg",
        "The Grand Duchy of Luxembourg",
        "UN member state",
        "LU",
        "LUX",
        442,
        ".lu"
    );

    public static CountryInfo Macao { get; } = new(
        "Macao",
        "The Macao Special Administrative Region of China",
        "China",
        "MO",
        "MAC",
        446,
        ".mo"
    );

    public static CountryInfo NorthMacedonia { get; } = new(
        "North Macedonia",
        "The Republic of North Macedonia",
        "UN member state",
        "MK",
        "MKD",
        807,
        ".mk"
    );

    public static CountryInfo Madagascar { get; } = new(
        "Madagascar",
        "The Republic of Madagascar",
        "UN member state",
        "MG",
        "MDG",
        450,
        ".mg"
    );

    public static CountryInfo Malawi { get; } = new(
        "Malawi",
        "The Republic of Malawi",
        "UN member state",
        "MW",
        "MWI",
        454,
        ".mw"
    );

    public static CountryInfo Malaysia { get; } = new(
        "Malaysia",
        "Malaysia",
        "UN member state",
        "MY",
        "MYS",
        458,
        ".my"
    );

    public static CountryInfo Maldives { get; } = new(
        "Maldives",
        "The Republic of Maldives",
        "UN member state",
        "MV",
        "MDV",
        462,
        ".mv"
    );

    public static CountryInfo Mali { get; } = new(
        "Mali",
        "The Republic of Mali",
        "UN member state",
        "ML",
        "MLI",
        466,
        ".ml"
    );

    public static CountryInfo Malta { get; } = new(
        "Malta",
        "The Republic of Malta",
        "UN member state",
        "MT",
        "MLT",
        470,
        ".mt"
    );

    public static CountryInfo MarshallIslands { get; } = new(
        "Marshall Islands (the)",
        "The Republic of the Marshall Islands",
        "UN member state",
        "MH",
        "MHL",
        584,
        ".mh"
    );

    public static CountryInfo Martinique { get; } = new(
        "Martinique",
        "Martinique",
        "France",
        "MQ",
        "MTQ",
        474,
        ".mq"
    );

    public static CountryInfo Mauritania { get; } = new(
        "Mauritania",
        "The Islamic Republic of Mauritania",
        "UN member state",
        "MR",
        "MRT",
        478,
        ".mr"
    );

    public static CountryInfo Mauritius { get; } = new(
        "Mauritius",
        "The Republic of Mauritius",
        "UN member state",
        "MU",
        "MUS",
        480,
        ".mu"
    );

    public static CountryInfo Mayotte { get; } = new(
        "Mayotte",
        "The Department of Mayotte",
        "France",
        "YT",
        "MYT",
        175,
        ".yt"
    );

    public static CountryInfo Mexico { get; } = new(
        "Mexico",
        "The United Mexican States",
        "UN member state",
        "MX",
        "MEX",
        484,
        ".mx"
    );

    public static CountryInfo Micronesia { get; } = new(
        "Micronesia (Federated States of)",
        "The Federated States of Micronesia",
        "UN member state",
        "FM",
        "FSM",
        583,
        ".fm"
    );

    public static CountryInfo Moldova { get; } = new(
        "Moldova (the Republic of)",
        "The Republic of Moldova",
        "UN member state",
        "MD",
        "MDA",
        498,
        ".md"
    );

    public static CountryInfo Monaco { get; } = new(
        "Monaco",
        "The Principality of Monaco",
        "UN member state",
        "MC",
        "MCO",
        492,
        ".mc"
    );

    public static CountryInfo Mongolia { get; } = new(
        "Mongolia",
        "Mongolia",
        "UN member state",
        "MN",
        "MNG",
        496,
        ".mn"
    );

    public static CountryInfo Montenegro { get; } = new(
        "Montenegro",
        "Montenegro",
        "UN member state",
        "ME",
        "MNE",
        499,
        ".me"
    );

    public static CountryInfo Montserrat { get; } = new(
        "Montserrat",
        "Montserrat",
        "United Kingdom",
        "MS",
        "MSR",
        500,
        ".ms"
    );

    public static CountryInfo Morocco { get; } = new(
        "Morocco",
        "The Kingdom of Morocco",
        "UN member state",
        "MA",
        "MAR",
        504,
        ".ma"
    );

    public static CountryInfo Mozambique { get; } = new(
        "Mozambique",
        "The Republic of Mozambique",
        "UN member state",
        "MZ",
        "MOZ",
        508,
        ".mz"
    );

    public static CountryInfo Myanmar { get; } = new(
        "Myanmar",
        "The Republic of the Union of Myanmar",
        "UN member state",
        "MM",
        "MMR",
        104,
        ".mm"
    );

    public static CountryInfo Namibia { get; } = new(
        "Namibia",
        "The Republic of Namibia",
        "UN member state",
        "NA",
        "NAM",
        516,
        ".na"
    );

    public static CountryInfo Nauru { get; } = new(
        "Nauru",
        "The Republic of Nauru",
        "UN member state",
        "NR",
        "NRU",
        520,
        ".nr"
    );

    public static CountryInfo Nepal { get; } = new(
        "Nepal",
        "The Federal Democratic Republic of Nepal",
        "UN member state",
        "NP",
        "NPL",
        524,
        ".np"
    );

    public static CountryInfo NetherlandsKingdomofthe { get; } = new(
        "Netherlands, Kingdom of the",
        "The Kingdom of the Netherlands",
        "UN member state",
        "NL",
        "NLD",
        528,
        ".nl"
    );

    public static CountryInfo NewCaledonia { get; } = new(
        "New Caledonia",
        "New Caledonia",
        "France",
        "NC",
        "NCL",
        540,
        ".nc"
    );

    public static CountryInfo NewZealand { get; } = new(
        "New Zealand",
        "New Zealand",
        "UN member state",
        "NZ",
        "NZL",
        554,
        ".nz"
    );

    public static CountryInfo Nicaragua { get; } = new(
        "Nicaragua",
        "The Republic of Nicaragua",
        "UN member state",
        "NI",
        "NIC",
        558,
        ".ni"
    );

    public static CountryInfo Niger { get; } = new(
        "Niger (the)",
        "The Republic of the Niger",
        "UN member state",
        "NE",
        "NER",
        562,
        ".ne"
    );

    public static CountryInfo Nigeria { get; } = new(
        "Nigeria",
        "The Federal Republic of Nigeria",
        "UN member state",
        "NG",
        "NGA",
        566,
        ".ng"
    );

    public static CountryInfo Niue { get; } = new(
        "Niue",
        "Niue",
        "New Zealand",
        "NU",
        "NIU",
        570,
        ".nu"
    );

    public static CountryInfo NorfolkIsland { get; } = new(
        "Norfolk Island",
        "The Territory of Norfolk Island",
        "Australia",
        "NF",
        "NFK",
        574,
        ".nf"
    );

    public static CountryInfo NorthernMarianaIslands { get; } = new(
        "Northern Mariana Islands (the)",
        "The Commonwealth of the Northern Mariana Islands",
        "United States",
        "MP",
        "MNP",
        580,
        ".mp"
    );

    public static CountryInfo Norway { get; } = new(
        "Norway",
        "The Kingdom of Norway",
        "UN member state",
        "NO",
        "NOR",
        578,
        ".no"
    );

    public static CountryInfo Oman { get; } = new(
        "Oman",
        "The Sultanate of Oman",
        "UN member state",
        "OM",
        "OMN",
        512,
        ".om"
    );

    public static CountryInfo Pakistan { get; } = new(
        "Pakistan",
        "The Islamic Republic of Pakistan",
        "UN member state",
        "PK",
        "PAK",
        586,
        ".pk"
    );

    public static CountryInfo Palau { get; } = new(
        "Palau",
        "The Republic of Palau",
        "UN member state",
        "PW",
        "PLW",
        585,
        ".pw"
    );

    public static CountryInfo PalestineStateof { get; } = new(
        "Palestine, State of",
        "The State of Palestine",
        "UN observer state",
        "PS",
        "PSE",
        275,
        ".ps"
    );

    public static CountryInfo Panama { get; } = new(
        "Panama",
        "The Republic of PanamÃ¡",
        "UN member state",
        "PA",
        "PAN",
        591,
        ".pa"
    );

    public static CountryInfo PapuaNewGuinea { get; } = new(
        "Papua New Guinea",
        "The Independent State of Papua New Guinea",
        "UN member state",
        "PG",
        "PNG",
        598,
        ".pg"
    );

    public static CountryInfo Paraguay { get; } = new(
        "Paraguay",
        "The Republic of Paraguay",
        "UN member state",
        "PY",
        "PRY",
        600,
        ".py"
    );

    public static CountryInfo Peru { get; } = new(
        "Peru",
        "The Republic of PerÃº",
        "UN member state",
        "PE",
        "PER",
        604,
        ".pe"
    );

    public static CountryInfo Philippines { get; } = new(
        "Philippines (the)",
        "The Republic of the Philippines",
        "UN member state",
        "PH",
        "PHL",
        608,
        ".ph"
    );

    public static CountryInfo Pitcairn { get; } = new(
        "Pitcairn",
        "The Pitcairn, Henderson, Ducie and Oeno Islands",
        "United Kingdom",
        "PN",
        "PCN",
        612,
        ".pn"
    );

    public static CountryInfo Poland { get; } = new(
        "Poland",
        "The Republic of Poland",
        "UN member state",
        "PL",
        "POL",
        616,
        ".pl"
    );

    public static CountryInfo Portugal { get; } = new(
        "Portugal",
        "The Portuguese Republic",
        "UN member state",
        "PT",
        "PRT",
        620,
        ".pt"
    );

    public static CountryInfo PuertoRico { get; } = new(
        "Puerto Rico",
        "The Commonwealth of Puerto Rico",
        "United States",
        "PR",
        "PRI",
        630,
        ".pr"
    );

    public static CountryInfo Qatar { get; } = new(
        "Qatar",
        "The State of Qatar",
        "UN member state",
        "QA",
        "QAT",
        634,
        ".qa"
    );

    public static CountryInfo Reunion { get; } = new(
        "Réunion",
        "Réunion",
        "France",
        "RE",
        "REU",
        638,
        ".re"
    );

    public static CountryInfo Romania { get; } = new(
        "Romania",
        "Romania",
        "UN member state",
        "RO",
        "ROU",
        642,
        ".ro"
    );

    public static CountryInfo RussianFederation { get; } = new(
        "Russian Federation (the)",
        "The Russian Federation",
        "UN member state",
        "RU",
        "RUS",
        643,
        ".ru"
    );

    public static CountryInfo Rwanda { get; } = new(
        "Rwanda",
        "The Republic of Rwanda",
        "UN member state",
        "RW",
        "RWA",
        646,
        ".rw"
    );

    public static CountryInfo SaintBarthelemy { get; } = new(
        "Saint Barthélemy",
        "The Collectivity of Saint-Barthélemy",
        "France",
        "BL",
        "BLM",
        652,
        ".bl"
    );

    public static CountryInfo SaintHelena { get; } = new(
        "Saint Helena, Ascension Island, Tristan da Cunha",
        "Saint Helena, Ascension and Tristan da Cunha",
        "United Kingdom",
        "SH",
        "SHN",
        654,
        ".sh"
    );

    public static CountryInfo SaintKittsAndNevis { get; } = new(
        "Saint Kitts and Nevis",
        "Saint Kitts and Nevis",
        "UN member state",
        "KN",
        "KNA",
        659,
        ".kn"
    );

    public static CountryInfo SaintLucia { get; } = new(
        "Saint Lucia",
        "Saint Lucia",
        "UN member state",
        "LC",
        "LCA",
        662,
        ".lc"
    );

    public static CountryInfo SaintMartin { get; } = new(
        "Saint Martin (French part)",
        "The Collectivity of Saint-Martin",
        "France",
        "MF",
        "MAF",
        663,
        ".mf"
    );

    public static CountryInfo SaintPierreandMiquelon { get; } = new(
        "Saint Pierre and Miquelon",
        "The Overseas Collectivity of Saint-Pierre and Miquelon",
        "France",
        "PM",
        "SPM",
        666,
        ".pm"
    );

    public static CountryInfo SaintVincentandtheGrenadines { get; } = new(
        "Saint Vincent and the Grenadines",
        "Saint Vincent and the Grenadines",
        "UN member state",
        "VC",
        "VCT",
        670,
        ".vc"
    );

    public static CountryInfo Samoa { get; } = new(
        "Samoa",
        "The Independent State of Samoa",
        "UN member state",
        "WS",
        "WSM",
        882,
        ".ws"
    );

    public static CountryInfo SanMarino { get; } = new(
        "San Marino",
        "The Republic of San Marino",
        "UN member state",
        "SM",
        "SMR",
        674,
        ".sm"
    );

    public static CountryInfo SaoTomeandPrincipe { get; } = new(
        "Sao Tome and Principe",
        "The Democratic Republic of SÃ£o TomÃ© and PrÃ­ncipe",
        "UN member state",
        "ST",
        "STP",
        678,
        ".st"
    );

    public static CountryInfo SaudiArabia { get; } = new(
        "Saudi Arabia",
        "The Kingdom of Saudi Arabia",
        "UN member state",
        "SA",
        "SAU",
        682,
        ".sa"
    );

    public static CountryInfo Senegal { get; } = new(
        "Senegal",
        "The Republic of Senegal",
        "UN member state",
        "SN",
        "SEN",
        686,
        ".sn"
    );

    public static CountryInfo Serbia { get; } = new(
        "Serbia",
        "The Republic of Serbia",
        "UN member state",
        "RS",
        "SRB",
        688,
        ".rs"
    );

    public static CountryInfo Seychelles { get; } = new(
        "Seychelles",
        "The Republic of Seychelles",
        "UN member state",
        "SC",
        "SYC",
        690,
        ".sc"
    );

    public static CountryInfo SierraLeone { get; } = new(
        "Sierra Leone",
        "The Republic of Sierra Leone",
        "UN member state",
        "SL",
        "SLE",
        694,
        ".sl"
    );

    public static CountryInfo Singapore { get; } = new(
        "Singapore",
        "The Republic of Singapore",
        "UN member state",
        "SG",
        "SGP",
        702,
        ".sg"
    );

    public static CountryInfo SintMaarten { get; } = new(
        "Sint Maarten (Dutch part)",
        "Sint Maarten",
        "Netherlands",
        "SX",
        "SXM",
        534,
        ".sx"
    );

    public static CountryInfo Slovakia { get; } = new(
        "Slovakia",
        "The Slovak Republic",
        "UN member state",
        "SK",
        "SVK",
        703,
        ".sk"
    );

    public static CountryInfo Slovenia { get; } = new(
        "Slovenia",
        "The Republic of Slovenia",
        "UN member state",
        "SI",
        "SVN",
        705,
        ".si"
    );

    public static CountryInfo SolomonIslands { get; } = new(
        "Solomon Islands",
        "The Solomon Islands",
        "UN member state",
        "SB",
        "SLB",
        090,
        ".sb"
    );

    public static CountryInfo Somalia { get; } = new(
        "Somalia",
        "The Federal Republic of Somalia",
        "UN member state",
        "SO",
        "SOM",
        706,
        ".so"
    );

    public static CountryInfo SouthAfrica { get; } = new(
        "South Africa",
        "The Republic of South Africa",
        "UN member state",
        "ZA",
        "ZAF",
        710,
        ".za"
    );

    public static CountryInfo SouthGeorgiaandtheSouthSandwichIslands { get; } = new(
        "South Georgia and the South Sandwich Islands",
        "South Georgia and the South Sandwich Islands",
        "United Kingdom",
        "GS",
        "SGS",
        239,
        ".gs"
    );

    public static CountryInfo SouthSudan { get; } = new(
        "South Sudan",
        "The Republic of South Sudan",
        "UN member state",
        "SS",
        "SSD",
        728,
        ".ss"
    );

    public static CountryInfo Spain { get; } = new(
        "Spain",
        "The Kingdom of Spain",
        "UN member state",
        "ES",
        "ESP",
        724,
        ".es"
    );

    public static CountryInfo SriLanka { get; } = new(
        "Sri Lanka",
        "The Democratic Socialist Republic of Sri Lanka",
        "UN member state",
        "LK",
        "LKA",
        144,
        ".lk"
    );

    public static CountryInfo Sudan { get; } = new(
        "Sudan (the)",
        "The Republic of the Sudan",
        "UN member state",
        "SD",
        "SDN",
        729,
        ".sd"
    );

    public static CountryInfo Suriname { get; } = new(
        "Suriname",
        "The Republic of Suriname",
        "UN member state",
        "SR",
        "SUR",
        740,
        ".sr"
    );

    public static CountryInfo SvalbardJanMayen { get; } = new(
        "Svalbard, Jan Mayen",
        "Svalbard and Jan Mayen",
        "Norway",
        "SJ",
        "SJM",
        744,
        ""
    );

    public static CountryInfo Sweden { get; } = new(
        "Sweden",
        "The Kingdom of Sweden",
        "UN member state",
        "SE",
        "SWE",
        752,
        ".se"
    );

    public static CountryInfo Switzerland { get; } = new(
        "Switzerland",
        "The Swiss Confederation",
        "UN member state",
        "CH",
        "CHE",
        756,
        ".ch"
    );

    public static CountryInfo SyrianArabRepublic { get; } = new(
        "Syrian Arab Republic (the)",
        "The Syrian Arab Republic",
        "UN member state",
        "SY",
        "SYR",
        760,
        ".sy"
    );

    public static CountryInfo Taiwan { get; } = new(
        "Taiwan (Province of China)",
        "The Republic of China",
        "Disputed",
        "TW",
        "TWN",
        158,
        ".tw"
    );

    public static CountryInfo Tajikistan { get; } = new(
        "Tajikistan",
        "The Republic of Tajikistan",
        "UN member state",
        "TJ",
        "TJK",
        762,
        ".tj"
    );

    public static CountryInfo TanzaniatheUnitedRepublicof { get; } = new(
        "Tanzania, the United Republic of",
        "The United Republic of Tanzania",
        "UN member state",
        "TZ",
        "TZA",
        834,
        ".tz"
    );

    public static CountryInfo Thailand { get; } = new(
        "Thailand",
        "The Kingdom of Thailand",
        "UN member state",
        "TH",
        "THA",
        764,
        ".th"
    );

    public static CountryInfo TimorLeste { get; } = new(
        "Timor-Leste",
        "The Democratic Republic of Timor-Leste",
        "UN member state",
        "TL",
        "TLS",
        626,
        ".tl"
    );

    public static CountryInfo Togo { get; } = new(
        "Togo",
        "The Togolese Republic",
        "UN member state",
        "TG",
        "TGO",
        768,
        ".tg"
    );

    public static CountryInfo Tokelau { get; } = new(
        "Tokelau",
        "Tokelau",
        "New Zealand",
        "TK",
        "TKL",
        772,
        ".tk"
    );

    public static CountryInfo Tonga { get; } = new(
        "Tonga",
        "The Kingdom of Tonga",
        "UN member state",
        "TO",
        "TON",
        776,
        ".to"
    );

    public static CountryInfo TrinidadandTobago { get; } = new(
        "Trinidad and Tobago",
        "The Republic of Trinidad and Tobago",
        "UN member state",
        "TT",
        "TTO",
        780,
        ".tt"
    );

    public static CountryInfo Tunisia { get; } = new(
        "Tunisia",
        "The Republic of Tunisia",
        "UN member state",
        "TN",
        "TUN",
        788,
        ".tn"
    );

    public static CountryInfo Turkey { get; } = new(
        "Türkiye",
        "The Republic of Türkiye",
        "UN member state",
        "TR",
        "TUR",
        792,
        ".tr"
    );

    public static CountryInfo Turkmenistan { get; } = new(
        "Turkmenistan",
        "Turkmenistan",
        "UN member state",
        "TM",
        "TKM",
        795,
        ".tm"
    );

    public static CountryInfo TurksandCaicosIslands { get; } = new(
        "Turks and Caicos Islands (the)",
        "The Turks and Caicos Islands",
        "United Kingdom",
        "TC",
        "TCA",
        796,
        ".tc"
    );

    public static CountryInfo Tuvalu { get; } = new(
        "Tuvalu",
        "Tuvalu",
        "UN member state",
        "TV",
        "TUV",
        798,
        ".tv"
    );

    public static CountryInfo Uganda { get; } = new(
        "Uganda",
        "The Republic of Uganda",
        "UN member state",
        "UG",
        "UGA",
        800,
        ".ug"
    );

    public static CountryInfo Ukraine { get; } = new(
        "Ukraine",
        "Ukraine",
        "UN member state",
        "UA",
        "UKR",
        804,
        ".ua"
    );

    public static CountryInfo UnitedArabEmirates { get; } = new(
        "United Arab Emirates (the)",
        "The United Arab Emirates",
        "UN member state",
        "AE",
        "ARE",
        784,
        ".ae"
    );

    public static CountryInfo UnitedKingdomofGreatBritainandNorthernIreland { get; } = new(
        "United Kingdom of Great Britain and Northern Ireland (the)",
        "The United Kingdom of Great Britain and Northern Ireland",
        "UN member state",
        "GB",
        "GBR",
        826,
        ".gb .uk"
    );

    public static CountryInfo UnitedStatesMinorOutlyingIslands { get; } = new(
        "United States Minor Outlying Islands (the)",
        "BakerÂ Island, HowlandÂ Island, JarvisÂ Island, JohnstonÂ Atoll, KingmanÂ Reef, MidwayÂ Atoll, NavassaÂ Island, PalmyraÂ Atoll, and WakeÂ Island",
        "United States",
        "UM",
        "UMI",
        581,
        ""
    );

    public static CountryInfo UnitedStatesofAmerica { get; } = new(
        "United States of America (the)",
        "The United States of America",
        "UN member state",
        "US",
        "USA",
        840,
        ".us"
    );

    public static CountryInfo Uruguay { get; } = new(
        "Uruguay",
        "The Oriental Republic of Uruguay",
        "UN member state",
        "UY",
        "URY",
        858,
        ".uy"
    );

    public static CountryInfo Uzbekistan { get; } = new(
        "Uzbekistan",
        "The Republic of Uzbekistan",
        "UN member state",
        "UZ",
        "UZB",
        860,
        ".uz"
    );

    public static CountryInfo Vanuatu { get; } = new(
        "Vanuatu",
        "The Republic of Vanuatu",
        "UN member state",
        "VU",
        "VUT",
        548,
        ".vu"
    );

    public static CountryInfo Venezuela { get; } = new(
        "Venezuela (Bolivarian Republic of)",
        "The Bolivarian Republic of Venezuela",
        "UN member state",
        "VE",
        "VEN",
        862,
        ".ve"
    );

    public static CountryInfo VietNam { get; } = new(
        "Viet Nam",
        "The Socialist Republic of Viet Nam",
        "UN member state",
        "VN",
        "VNM",
        704,
        ".vn"
    );

    public static CountryInfo BritishVirginIslands { get; } = new(
        "Virgin Islands (British)",
        "The Virgin Islands",
        "United Kingdom",
        "VG",
        "VGB",
        092,
        ".vg"
    );

    public static CountryInfo USVirginIslands { get; } = new(
        "Virgin Islands (U.S.)",
        "The Virgin Islands of the United States",
        "United States",
        "VI",
        "VIR",
        850,
        ".vi"
    );

    public static CountryInfo WallisandFutuna { get; } = new(
        "Wallis and Futuna",
        "The Territory of the Wallis and Futuna Islands",
        "France",
        "WF",
        "WLF",
        876,
        ".wf"
    );

    public static CountryInfo WesternSahara { get; } = new(
        "Western Sahara",
        "The Sahrawi Arab Democratic Republic",
        "Disputed",
        "EH",
        "ESH",
        732,
        ""
    );

    public static CountryInfo Yemen { get; } = new(
        "Yemen",
        "The Republic of Yemen",
        "UN member state",
        "YE",
        "YEM",
        887,
        ".ye"
    );

    public static CountryInfo Zambia { get; } = new(
        "Zambia",
        "The Republic of Zambia",
        "UN member state",
        "ZM",
        "ZMB",
        894,
        ".zm"
    );

    public static CountryInfo Zimbabwe { get; } = new(
        "Zimbabwe",
        "The Republic of Zimbabwe",
        "UN member state",
        "ZW",
        "ZWE",
        716,
        ".zw"
    );
}