using Newtonsoft.Json;

public class Program
{
    public static void Main()
    {
        // Primeiro teste: Paris Saint-Germain em 2013
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Segundo teste: Chelsea em 2014
        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    /// <summary>
    /// Calcula o total de gols marcados por um time em um determinado ano,
    /// considerando jogos em que o time foi o mandante (team1) e o visitante (team2).
    /// </summary>
    public static int getTotalScoredGoals(string teamName, int year)
    {
        int totalGoals = 0;
        string apiUrl = "https://jsonmock.hackerrank.com/api/football_matches";

        using (HttpClient httpClient = new HttpClient())
        {
            // Soma os gols quando o time foi o mandante
            totalGoals += GetGoalsForTeamSide(httpClient, apiUrl, teamName, year, teamQueryParam: "team1", goalPropertyName: "team1goals");

            // Soma os gols quando o time foi o visitante
            totalGoals += GetGoalsForTeamSide(httpClient, apiUrl, teamName, year, teamQueryParam: "team2", goalPropertyName: "team2goals");
        }

        return totalGoals;
    }

    /// <summary>
    /// Consulta a API de partidas e soma os gols marcados por um time,
    /// considerando apenas o lado (team1 ou team2) e o campo de gols correspondente.
    /// </summary>
    /// <param name="httpClient">Cliente HTTP usado para fazer requisições</param>
    /// <param name="apiUrl">URL base da API</param>
    /// <param name="teamName">Nome do time a ser consultado</param>
    /// <param name="year">Ano da pesquisa</param>
    /// <param name="teamQueryParam">Parâmetro a ser usado na query: "team1" ou "team2"</param>
    /// <param name="goalPropertyName">Campo de gols: "team1goals" ou "team2goals"</param>
    /// <returns>Total de gols marcados pelo time nesse lado</returns>
    public static int GetGoalsForTeamSide(HttpClient httpClient, string apiUrl, string teamName, int? year, string teamQueryParam, string goalPropertyName)
    {
        int totalGoals = 0;
        int currentPage = 1;
        int totalPages = 1;

        do
        {
            // Monta dinamicamente a query string com os parâmetros fornecidos
            var queryParams = new List<string>();

            if (year.HasValue)
                queryParams.Add($"year={year}");

            if (!string.IsNullOrWhiteSpace(teamName))
                queryParams.Add($"{teamQueryParam}={Uri.EscapeDataString(teamName)}");

            queryParams.Add($"page={currentPage}");

            string requestUrl = $"{apiUrl}?{string.Join("&", queryParams)}";

            // Faz a requisição HTTP
            var response = httpClient.GetAsync(requestUrl).Result;
            var jsonResponse = response.Content.ReadAsStringAsync().Result;

            var matchData = JsonConvert.DeserializeObject<FootballMatchResponse>(jsonResponse);

            foreach (var match in matchData.matches)
            {
                if (match.TryGetValue(goalPropertyName, out string goalsStr) && int.TryParse(goalsStr, out int goals))
                {
                    totalGoals += goals;
                }
            }

            totalPages = matchData.total_pages;
            currentPage++;

        } while (currentPage <= totalPages);

        return totalGoals;
    }


    /// <summary>
    /// Representa a estrutura de resposta da API de partidas.
    /// O campo 'data' é renomeado para 'matches' para melhor clareza.
    /// </summary>
    public class FootballMatchResponse
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }

        [JsonProperty("data")]
        public List<Dictionary<string, string>> matches { get; set; }
    }
}