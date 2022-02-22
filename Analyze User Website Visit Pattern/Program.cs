using System;
using System.Collections.Generic;

namespace Analyze_User_Website_Visit_Pattern
{
  class Program
  {
    static void Main(string[] args)
    {
      Program p = new Program();
      string[] username = new string[] { "joe", "joe", "joe", "james", "james", "james", "james", "mary", "mary", "mary" };
      int[] timestamp = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      string[] website = new string[] { "ahome", "about", "career", "home", "cart", "maps", "home", "aahome", "about", "career" };
      var result = p.MostVisitedWebsites(username, timestamp, website);
      Console.WriteLine(string.Join(",", result));
    }


    // Input: username = ["joe","joe","joe","james","james","james","james","mary","mary","mary"],
    // timestamp = [1,2,3,4,5,6,7,8,9,10],
    // website = ["home","about","career","home","cart","maps","home","home","about","career"]
    /*["joe", 1, "ahome"]
      ["joe", 2, "about"]
      ["joe", 3, "career"]
      ["james", 4, "home"]
      ["james", 5, "cart"]
      ["james", 6, "maps"]
      ["james", 7, "home"]
      ["mary", 8, "aahome"]
      ["mary", 9, "about"]
      ["mary", 10, "career"]
    */
    List<string> MostVisitedWebsites(string[] username, int[] timestamp, string[] website)
    {
      // step 1 - create a map for each user and sorted map <time, websites>. We need a sorted map as the output needs to be in ascending order by the time of their visits.
      // Step 2 - For each user create the 3 sites unique string and count visit occurance.
      // Step 3 - When there are multiple 3 visited sites key count is same, will be sorting the 3 site visit key lexicographically smallest.

      // Step - 1
      List<string> result = new List<string>();
      Dictionary<string, SortedDictionary<int, string>> userTimeVisitedSitesMap = new Dictionary<string, SortedDictionary<int, string>>();
      for (int i = 0; i < timestamp.Length; i++)
      {
        string user = username[i];
        int visitedTime = timestamp[i];
        string visitedWebSite = website[i];
        if (!userTimeVisitedSitesMap.ContainsKey(user))
        {
          userTimeVisitedSitesMap.Add(user, new SortedDictionary<int, string>());
        }

        var visitedTimeAndSiteMap = userTimeVisitedSitesMap[user];
        visitedTimeAndSiteMap.Add(visitedTime, visitedWebSite);
        userTimeVisitedSitesMap[user] = visitedTimeAndSiteMap;
      }

      // Step - 2
      Dictionary<string, int> siteVisitFrequency = new Dictionary<string, int>();
      foreach (var kvp in userTimeVisitedSitesMap)
      {
        if (kvp.Key.Length >= 3)
        {
          var visitedTimeAndSiteMap = kvp.Value;
          var times = visitedTimeAndSiteMap.Keys;
          List<int> visitedTimes = new List<int>();
          foreach (var time in times)
          {
            visitedTimes.Add(time);
          }

          var visitedSiteKeys = CreateSiteVisitKey(visitedTimeAndSiteMap, visitedTimes);
          HashSet<string> visitedSitesForSameUser = new HashSet<string>();
          foreach (var key in visitedSiteKeys)
          {
            if (visitedSitesForSameUser.Add(key))
            {
              if (!siteVisitFrequency.ContainsKey(key))
              {
                siteVisitFrequency.Add(key, 1);
              }
              else
              {
                siteVisitFrequency[key] += 1;
              }
            }
          }
        }
      }

      // Step - 3
      int maxCount = 0;
      string maxCountSiteVisitKey = string.Empty;
      foreach(var freq in siteVisitFrequency)
      {
        if(freq.Value > maxCount)
        {
          maxCount = freq.Value;
          maxCountSiteVisitKey = freq.Key;
        }
        else if(freq.Value == maxCount)
        {
          // update maxCountSiteVisitKey based on lexicographically smallest.
          if(freq.Key.CompareTo(maxCountSiteVisitKey) < 0)
          {
            maxCountSiteVisitKey = freq.Key;
          }
        }
      }

      foreach(string answer in maxCountSiteVisitKey.Split("->"))
      {
        result.Add(answer);
      }

      return result;
    }

    private List<string> CreateSiteVisitKey(SortedDictionary<int, string> visitedTimeAndSiteMap, List<int> times)
    {
      List<string> threeSiteVisitKey = new List<string>();
      for (int i = 0; i < times.Count - 2; i++)
      {
        for (int j = i+1; j < times.Count - 1; j++)
        {
          for (int k = j+1; k < times.Count; k++)
          {
            string key = $"{visitedTimeAndSiteMap[times[i]]}->{visitedTimeAndSiteMap[times[j]]}->{visitedTimeAndSiteMap[times[k]]}";
            threeSiteVisitKey.Add(key);
          }
        }
      }

      return threeSiteVisitKey;
    }
  }
}
