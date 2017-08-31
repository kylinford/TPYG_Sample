using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class CVMDataStorage : JWMonoBehaviour {
    string[] courseReq;
    string[] courseNames;

    public string[] CourseReq { get { return courseReq; } }
    public string[] CourseNames { get { return courseNames; } }


    // Use this for initialization
    void Start () {
        //csvConvert("MajorCourseRequirementSpreadsheet.csv");
        SetDataArray();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<string> getCourseReq(string major)
    {
        List<string> ret = new List<string>();
        int eachRowCount = courseNames.Length;
        for (int i=0; i< courseReq.Length / eachRowCount; i++)
        {
            if(courseReq[i* eachRowCount] == major)
            {
                for (int j=1;j<= courseNames.Length-2; j++)
                {
                    if (courseReq[i * eachRowCount + j] != "0")
                    {
                        int currCount = Int32.Parse(courseReq[i * eachRowCount + j]);
                        for(int k = 0; k < currCount; k++)
                        {
                            ret.Add(courseNames[j]);
                        }
                    }
                }
            }

        }
        return ret;

    }

    public string GetPurdueMajor(int i)
    {
        return courseReq[i * courseNames.Length];
    }

    void SetDataArray()
    {
        courseNames = new string[]
        {
            "PurdueMajor", "Agriculture", "Art", "Biology", "Business", "Chemistry", "Computer Science", "Education", "Engineering", "English and Communication", "Family and Consumer Science", "Foreign Language", "Health", "History and Culture", "Math and Stats", "Physics", "Social Science", "Technology", "Free Electives", "PurdueMajor"
        };
        //Groups (still need update)
        courseReq = new string[]{
            //PurdueMajor,Agriculture,Art,Biology,Business,Chemistry,Computer Science,Education,Engineering,English and Communication,Family and Consumer Science,Foreign Language,Health,History and Culture,Math and Stats,Physics,Social Science,Technology,Free Electives,PurdueMajor
            "Actuarial Science", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "4", "0", "1", "0", "2", "Actuarial Science",
            "Aeronautical Engineering Technology", "0", "0", "0", "0", "0", "0", "0", "0", "3", "0", "0", "0", "0", "2", "0", "1", "1", "1", "Aeronautical Engineering Technology",
            "Aeronautics and Astronautics", "0", "0", "0", "0", "0", "0", "0", "7", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "Aeronautics and Astronautics",
            "Agribusiness", "6", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "1", "0", "0", "Agribusiness",
            "Agricultural Communication", "4", "0", "0", "0", "0", "0", "0", "0", "3", "0", "0", "0", "0", "0", "0", "0", "0", "1", "Agricultural Communication",
            "Agricultural Economics", "6", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "1", "0", "0", "Agricultural Economics",
            "Agricultural Engineering", "0", "0", "0", "0", "0", "0", "0", "6", "1", "0", "0", "0", "0", "1", "0", "0", "0", "0", "Agricultural Engineering",
            "Agricultural Systems Management", "7", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "Agricultural Systems Management",
            "Agricultural Teacher Education", "5", "0", "0", "0", "0", "0", "3", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "Agricultural Teacher Education",
            "Agriculture", "4", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "3", "Agriculture",
            "Agronomy", "3", "0", "0", "0", "1", "0", "0", "0", "1", "0", "0", "0", "0", "1", "1", "0", "0", "1", "Agronomy",
            "Animal Sciences", "4", "0", "1", "0", "1", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "0", "0", "Animal Sciences",
            "Anthropology", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "5", "0", "0", "1", "0", "0", "Anthropology",
            "Applied Exercise and Health", "0", "0", "1", "0", "0", "0", "0", "0", "1", "0", "0", "4", "0", "0", "0", "1", "0", "1", "Applied Exercise and Health",
            "Art Studies", "0", "5", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "1", "0", "0", "1", "0", "0", "Art Studies",
            "Art Teacher Education", "0", "6", "0", "0", "0", "0", "2", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "Art Teacher Education",
            "Aviation Management", "0", "0", "0", "2", "0", "0", "0", "0", "3", "0", "0", "0", "0", "1", "0", "2", "0", "0", "Aviation Management",
            "Biochemistry", "0", "0", "2", "0", "3", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "2", "Biochemistry",
            "Biological Engineering", "1", "0", "1", "0", "0", "0", "0", "4", "1", "0", "0", "0", "0", "1", "0", "0", "0", "0", "Biological Engineering",
            "Biology", "0", "0", "4", "0", "1", "0", "0", "0", "0", "0", "1", "0", "0", "1", "0", "1", "0", "0", "Biology",
            "Biomedical Engineering", "0", "0", "1", "0", "0", "0", "0", "6", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "Biomedical Engineering",
            "Brain and Behavioral Sciences", "0", "0", "1", "0", "0", "0", "0", "0", "1", "0", "1", "0", "1", "0", "0", "4", "0", "0", "Brain and Behavioral Sciences",
            "Building Construction Management", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "6", "1", "Building Construction Management",
            "Chemical Engineering", "0", "0", "0", "0", "1", "0", "0", "5", "0", "0", "0", "0", "0", "2", "0", "0", "0", "0", "Chemical Engineering",
            "Chemistry", "0", "0", "0", "0", "4", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "2", "Chemistry",
            "Civil Engineering", "0", "0", "0", "0", "0", "0", "0", "7", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "Civil Engineering",
            "Communication", "0", "1", "0", "0", "0", "0", "0", "0", "4", "0", "1", "0", "1", "0", "0", "1", "0", "0", "Communication",
            "Computer Engineering", "0", "0", "0", "0", "0", "0", "0", "6", "0", "0", "0", "0", "0", "1", "0", "1", "0", "0", "Computer Engineering",
            "Computer Engineering Technology", "0", "0", "0", "0", "0", "5", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "1", "0", "Computer Engineering Technology",
            "Computer Graphics Technology", "0", "1", "0", "1", "0", "1", "0", "0", "2", "0", "0", "0", "0", "1", "0", "1", "0", "1", "Computer Graphics Technology",
            "Computer Science", "0", "0", "0", "0", "0", "4", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "2", "Computer Science",
            "Computer and Information Technology", "0", "0", "0", "0", "0", "4", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "1", "1", "Computer and Information Technology",
            "Construction Engineering", "0", "0", "0", "0", "0", "0", "0", "6", "0", "0", "0", "0", "0", "1", "0", "0", "0", "1", "Construction Engineering",
            "Creative Writing", "0", "0", "0", "0", "0", "0", "0", "0", "5", "0", "1", "0", "1", "0", "0", "1", "0", "0", "Creative Writing",
            "Design Studies", "0", "6", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "Design Studies",
            "Developmental and Family Science", "0", "0", "0", "0", "0", "0", "0", "0", "1", "6", "0", "0", "0", "0", "0", "1", "0", "0", "Developmental and Family Science",
            "Early Childhood Education", "0", "0", "0", "0", "0", "0", "1", "0", "1", "6", "0", "0", "0", "0", "0", "0", "0", "0", "Early Childhood Education",
            "Earth Atmospheric and Planetary Sciences", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "6", "0", "0", "1", "Earth Atmospheric and Planetary Sciences",
            "Economics", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "4", "0", "1", "Economics",
            "Electrical Engineering", "0", "0", "0", "0", "0", "0", "0", "6", "0", "0", "0", "0", "0", "1", "0", "1", "0", "0", "Electrical Engineering",
            "Electrical Engineering Technology", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "6", "0", "Electrical Engineering Technology",
            "Elementary Education", "0", "0", "0", "0", "0", "0", "4", "0", "1", "0", "0", "0", "0", "1", "0", "0", "0", "2", "Elementary Education",
            "English", "0", "0", "0", "0", "0", "0", "0", "0", "5", "0", "1", "0", "1", "0", "0", "1", "0", "0", "English",
            "English Teacher Education", "0", "0", "0", "0", "0", "0", "2", "0", "6", "0", "0", "0", "0", "0", "0", "0", "0", "0", "English Teacher Education",
            "Entomology", "6", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "Entomology",
            "Environmental Engineering", "0", "0", "0", "0", "0", "0", "0", "4", "0", "0", "0", "0", "0", "1", "0", "0", "0", "3", "Environmental Engineering",
            "Environmental Health Sciences", "0", "0", "1", "0", "1", "0", "0", "0", "1", "0", "0", "3", "0", "1", "0", "1", "0", "0", "Environmental Health Sciences",
            "Family and Consumer Science Education", "0", "0", "0", "0", "0", "0", "4", "0", "0", "2", "0", "1", "0", "0", "0", "0", "0", "1", "Family and Consumer Science Education",
            "Farm Management", "7", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "Farm Management",
            "Film and Video Studies", "0", "1", "0", "0", "0", "0", "0", "0", "2", "0", "1", "0", "3", "0", "0", "1", "0", "0", "Film and Video Studies",
            "Financial Counseling and Planning", "1", "0", "0", "1", "0", "0", "0", "0", "1", "3", "0", "0", "0", "1", "0", "1", "0", "0", "Financial Counseling and Planning",
            "Fisheries and Aquatic Sciences", "7", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "Fisheries and Aquatic Sciences",
            "Food Science", "4", "0", "0", "0", "1", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "0", "1", "Food Science",
            "Foreign Language Teacher Education", "0", "0", "0", "0", "0", "0", "2", "0", "1", "0", "4", "0", "0", "0", "0", "0", "0", "1", "Foreign Language Teacher Education",
            "Forestry", "6", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "0", "0", "Forestry",
            "Genetics", "0", "0", "4", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "2", "Genetics",
            "Health Sciences General", "0", "0", "1", "0", "1", "0", "0", "0", "1", "0", "0", "2", "0", "1", "0", "1", "0", "1", "Health Sciences General",
            "Health Sciences Pre-Professional", "0", "0", "1", "0", "1", "0", "0", "0", "1", "0", "0", "3", "0", "1", "0", "1", "0", "0", "Health Sciences Pre-Professional",
            "History", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "4", "0", "0", "1", "0", "1", "History",
            "Horticulture", "6", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "Horticulture",
            "Hospitality and Tourism Management", "0", "0", "0", "0", "0", "0", "0", "0", "1", "7", "0", "0", "0", "0", "0", "0", "0", "0", "Hospitality and Tourism Management",
            "Industrial Engineering", "0", "0", "0", "0", "0", "0", "0", "5", "0", "0", "0", "0", "0", "1", "0", "1", "0", "1", "Industrial Engineering",
            "Industrial Engineering Technology", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "1", "0", "4", "2", "Industrial Engineering Technology",
            "Industrial Management", "0", "0", "0", "4", "0", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "1", "0", "1", "Industrial Management",
            "Landscape Architecture", "7", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "Landscape Architecture",
            "Languages and Cultures", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "4", "0", "1", "0", "0", "1", "0", "1", "Languages and Cultures",
            "Law and Society", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "1", "0", "0", "4", "0", "1", "Law and Society",
            "Management", "0", "0", "0", "4", "0", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "1", "0", "1", "Management",
            "Manufacturing Engineering Technology", "0", "0", "0", "0", "0", "1", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "5", "0", "Manufacturing Engineering Technology",
            "Materials Engineering", "0", "0", "0", "0", "0", "0", "0", "7", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "Materials Engineering",
            "Mathematics", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "4", "0", "1", "0", "2", "Mathematics",
            "Mathematics Teacher Education", "0", "0", "0", "0", "0", "0", "2", "0", "0", "0", "0", "0", "0", "4", "0", "0", "0", "2", "Mathematics Teacher Education",
            "Mechanical Engineering", "0", "0", "0", "0", "0", "0", "0", "6", "0", "0", "0", "0", "0", "1", "0", "0", "0", "1", "Mechanical Engineering",
            "Mechanical Engineering Technology", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "6", "0", "Mechanical Engineering Technology",
            "Medical Laboratory Sciences", "0", "0", "1", "0", "1", "0", "0", "0", "0", "0", "0", "6", "0", "0", "0", "0", "0", "0", "Medical Laboratory Sciences",
            "Microbiology and Molecular Biology", "0", "0", "4", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "2", "Microbiology and Molecular Biology",
            "Movement and Sports Sciences", "0", "0", "1", "0", "0", "0", "0", "0", "1", "0", "0", "4", "1", "0", "0", "1", "0", "0", "Movement and Sports Sciences",
            "Multidisciplinary Engineering", "0", "1", "0", "0", "0", "0", "0", "4", "0", "0", "0", "0", "0", "1", "0", "0", "0", "2", "Multidisciplinary Engineering",
            "Natural Resources and Environmental Science", "4", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "1", "0", "1", "Natural Resources and Environmental Science",
            "Neurobiology and Physiology", "0", "0", "4", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "1", "Neurobiology and Physiology",
            "Nuclear Engineering", "0", "0", "0", "0", "0", "0", "0", "7", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "Nuclear Engineering",
            "Nursing", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "7", "0", "0", "0", "1", "0", "0", "Nursing",
            "Nutrition Dietetics and Fitness", "0", "0", "1", "0", "1", "0", "0", "0", "1", "1", "0", "3", "0", "0", "0", "1", "0", "0", "Nutrition Dietetics and Fitness",
            "Organizational Leadership", "0", "0", "0", "1", "0", "0", "0", "0", "3", "0", "0", "0", "0", "1", "0", "1", "1", "1", "Organizational Leadership",
            "Philosophy", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "4", "0", "0", "1", "0", "1", "Philosophy",
            "Physical Education Teaching", "0", "0", "0", "0", "0", "0", "2", "0", "0", "0", "0", "6", "0", "0", "0", "0", "0", "0", "Physical Education Teaching",
            "Physics", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "2", "4", "0", "0", "2", "Physics",
            "Political Science", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "1", "0", "0", "4", "0", "1", "Political Science",
            "Professional Flight", "0", "0", "0", "0", "0", "0", "0", "0", "3", "0", "0", "0", "0", "1", "3", "1", "0", "0", "Professional Flight",
            "Professional Writing", "0", "0", "0", "0", "0", "0", "0", "0", "5", "0", "1", "0", "1", "0", "0", "1", "0", "0", "Professional Writing",
            "Psychology", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "1", "0", "0", "5", "0", "0", "Psychology",
            "Public Health", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "4", "0", "0", "0", "1", "0", "2", "Public Health",
            "Public Relations and Strategic Communication", "0", "0", "0", "0", "0", "0", "0", "0", "5", "0", "1", "0", "1", "0", "0", "1", "0", "0", "Public Relations and Strategic Communication",
            "Retail Management", "1", "0", "0", "1", "0", "0", "0", "0", "1", "4", "0", "0", "0", "0", "0", "1", "0", "0", "Retail Management",
            "Science Teacher Education", "0", "0", "1", "0", "1", "0", "2", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "2", "Science Teacher Education",
            "Selling and Sales Management", "1", "0", "0", "1", "0", "0", "0", "0", "1", "4", "0", "0", "0", "0", "0", "1", "0", "0", "Selling and Sales Management",
            "Social Studies Teacher Education", "0", "0", "0", "0", "0", "0", "2", "0", "0", "0", "0", "0", "2", "0", "0", "3", "0", "1", "Social Studies Teacher Education",
            "Sociology", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "1", "0", "0", "4", "0", "1", "Sociology",
            "Special Education", "0", "0", "0", "0", "0", "0", "7", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "Special Education",
            "Speech Language Hearing Sciences", "0", "0", "0", "0", "0", "0", "0", "0", "3", "0", "1", "0", "1", "0", "0", "2", "0", "1", "Speech Language Hearing Sciences",
            "Statistics", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "4", "0", "1", "0", "2", "Statistics",
            "Technology Teacher Education", "0", "0", "0", "0", "0", "0", "3", "0", "1", "0", "0", "0", "0", "0", "0", "0", "4", "0", "Technology Teacher Education",
            "Theatre", "0", "4", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "1", "0", "0", "1", "0", "0", "Theatre",
            "Turf Science", "4", "0", "0", "0", "1", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "0", "1", "Turf Science",
            "Veterinary Technology", "2", "0", "1", "0", "1", "0", "0", "0", "3", "0", "0", "0", "0", "1", "0", "0", "0", "0", "Veterinary Technology",
            "Wildlife", "7", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "Wildlife"
        };
    }

    List<List<string>> ReadCSV(string dir)
    {
        StreamReader reader = new StreamReader(File.OpenRead(dir));
        List<List<string>> ret = new List<List<string>>();

        while (!reader.EndOfStream)
        {
            List<string> row = new List<string>();
            string[] values = reader.ReadLine().Split(',');
            foreach (string value in values)
                row.Add(value);
            ret.Add(row);
        }

        return ret;
    }

    void csvConvert(string filename)
    {
        //MajorCourseRequirementSpreadsheet.csv
        List<List<string>> textInput;
        textInput = ReadCSV(@"Assets/" + filename);
        TextWriter tw = new StreamWriter(@"Assets/" + filename + ".output.csv");
        for (int i = 0; i < textInput.Count; i++)
        {
            string line = "";
            foreach (string value in textInput[i])
                line += "\"" + value + "\", ";
            tw.WriteLine(line);
        }
        tw.WriteLine(DateTime.Now);
        tw.Close();
    }


}
