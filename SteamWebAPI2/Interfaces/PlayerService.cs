﻿using SteamWebAPI2.Models.SteamCommunity;
using SteamWebAPI2.Models.SteamPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SteamWebAPI2.Interfaces
{
    public class PlayerService : SteamWebInterface, IPlayerService
    {
        public PlayerService(string steamWebApiKey)
            : base(steamWebApiKey, "IPlayerService")
        {
        }

        public async Task<PlayingSharedGameResult> IsPlayingSharedGameAsync(long steamId, int appId)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            AddToParametersIfHasValue(steamId, "steamid", parameters);
            AddToParametersIfHasValue(appId, "appid_playing", parameters);
            var playingSharedGameResult = await CallMethodAsync<PlayingSharedGameResultContainer>("IsPlayingSharedGame", 1, parameters);
            return playingSharedGameResult.Result;
        }

        public async Task<IReadOnlyCollection<BadgeQuest>> GetCommunityBadgeProgressAsync(long steamId, int? badgeId = null)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            AddToParametersIfHasValue(steamId, "steamid", parameters);
            AddToParametersIfHasValue(badgeId, "badgeid", parameters);
            var badgeProgressResult = await CallMethodAsync<CommunityBadgeProgressResultContainer>("GetCommunityBadgeProgress", 1, parameters);
            return new ReadOnlyCollection<BadgeQuest>(badgeProgressResult.Result.Quests);
        }

        public async Task<BadgesResult> GetBadgesAsync(long steamId)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            AddToParametersIfHasValue(steamId, "steamid", parameters);
            var badgesResult = await CallMethodAsync<BadgesResultContainer>("GetBadges", 1, parameters);
            return badgesResult.Result;
        }

        public async Task<int> GetSteamLevelAsync(long steamId)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            AddToParametersIfHasValue(steamId, "steamid", parameters);
            var steamLevelResult = await CallMethodAsync<SteamLevelResultContainer>("GetSteamLevel", 1, parameters);
            return steamLevelResult.Result.PlayerLevel;
        }

        public async Task<OwnedGamesResult> GetOwnedGamesAsync(long steamId, bool? includeAppInfo = null, bool? includeFreeGames = null, IReadOnlyCollection<int> appIdsToFilter = null)
        {
            int? includeAppInfoBit = 0;
            if (includeAppInfo.HasValue) { includeAppInfoBit = includeAppInfo.Value ? 1 : 0; }

            int? includeFreeGamesBit = 0;
            if (includeFreeGames.HasValue) { includeFreeGamesBit = includeFreeGames.Value ? 1 : 0; }

            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            AddToParametersIfHasValue(steamId, "steamid", parameters);
            AddToParametersIfHasValue(includeAppInfoBit, "include_appinfo", parameters);
            AddToParametersIfHasValue(includeFreeGamesBit, "include_played_Free_games", parameters);

            if (appIdsToFilter != null)
            {
                string appIdsDelimited = String.Join(",", appIdsToFilter);
                AddToParametersIfHasValue(appIdsDelimited, "appids_filter", parameters);
            }

            var ownedGamesResult = await CallMethodAsync<OwnedGamesResultContainer>("GetOwnedGames", 1, parameters);

            // for some reason, some games have trailing spaces in the result
            if (ownedGamesResult.Result != null && ownedGamesResult.Result.OwnedGames != null)
            {
                foreach (var ownedGame in ownedGamesResult.Result.OwnedGames)
                {
                    ownedGame.Name = ownedGame.Name.Trim();
                }
            }

            return ownedGamesResult.Result;
        }

        public async Task<RecentlyPlayedGameResult> GetRecentlyPlayedGamesAsync(long steamId)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            AddToParametersIfHasValue(steamId, "steamid", parameters);
            var recentlyPlayedGameResult = await CallMethodAsync<RecentlyPlayedGameResultContainer>("GetRecentlyPlayedGames", 1, parameters);
            return recentlyPlayedGameResult.Result;
        }
    }
}