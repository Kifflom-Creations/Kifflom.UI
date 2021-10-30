using System;
using System.Collections.Generic;
#if FIVEM
using CitizenFX.Core.Native;
#endif
using Kifflom.UI.Common;

namespace Kifflom.UI.Scaleforms
{
    /// <summary>
    /// Celebration processable to create and show a wall of items.
    /// </summary>
    public class Celebration : IProcessable
    {
        private readonly CelebrationLayer _layerMain;
        private readonly CelebrationLayer _layerBack;
        private readonly CelebrationLayer _layerFront;

        private readonly List<Action<CelebrationLayer>> _items;

        private bool _visible = true;

        /// <inheritdoc />
        public Celebration() : this(3)
        {

        }

        public Celebration(int duration)
        {
            _items = new List<Action<CelebrationLayer>>();

            _layerMain = new CelebrationLayer(CelebrationLayerEnum.Main, _items);
            _layerBack = new CelebrationLayer(CelebrationLayerEnum.Background, _items);
            _layerFront = new CelebrationLayer(CelebrationLayerEnum.Foreground, _items);
        }

        public int Duration
        {
            get => _layerMain.Duration;
            set
            {
                _layerMain.Duration = value;
                _layerBack.Duration = value;
                _layerFront.Duration = value;
            }
        }

        /// <inheritdoc />
        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                if (!value)
                {
                    _layerMain.Hide();
                    _layerBack.Hide();
                    _layerMain.Hide();
                }
            }
        }

        /// <inheritdoc />
        public void Process()
        {
            if (!Visible) return;

            if (!_layerMain.NeedsLoading)
            {
                _layerMain.Load();
                _layerBack.Load();
                _layerMain.Load();
            }
            else if (_layerMain.IsReady && _layerBack.IsReady && _layerFront.IsReady)
            {
                _layerMain.Show();
                _layerBack.Show();
                _layerMain.Show();
            }

            _layerBack.Process();
            _layerFront.Process();
            _layerMain.Process();
        }

        /// <summary>
        /// Remove all the items on the wall.
        /// </summary>
        public void Reset()
        {
            _items.Clear();
        }

        /// <summary>
        /// Add a Winner item to the wall.
        /// </summary>
        /// <param name="hasWon">Whether the player has won.</param>
        /// <param name="gamerName">The element that has been won (Game, Round, Match, etc).</param>
        /// <param name="teamName">The team that has won the game.</param>
        /// <param name="crewName">The crew that has won the game.</param>
        /// <param name="betAmount">The amount that the player has gained/lost in betting (0 = off).</param>
        /// <param name="rawGamerName">Whether the gamerName is rawText (null = automatic).</param>
        public void AddWinner(bool hasWon, string gamerName = "CELEB_ROUND", string teamName = "", string crewName = "", int betAmount = 0, bool? rawGamerName = null)
        {
            AddWinner(hasWon ? "CELEB_WINNER" : "CELEB_LOSER", gamerName, teamName, crewName, betAmount, rawGamerName);
        }

        /// <summary>
        /// Add a Winner item to the wall.
        /// </summary>
        /// <param name="winLose">The big text that displays a message.</param>
        /// <param name="gamerName">The element that has been won (Game, Round, Match, etc).</param>
        /// <param name="teamName">The team that has won the game.</param>
        /// <param name="crewName">The crew that has won the game.</param>
        /// <param name="betAmount">The amount that the player has gained/lost in betting (0 = off).</param>
        /// <param name="rawGamerName">Whether the gamerName is rawText (null = automatic).</param>
        public void AddWinner(string winLose, string gamerName = "CELEB_ROUND", string teamName = "", string crewName = "", int betAmount = 0, bool? rawGamerName = null)
        {
            if (!rawGamerName.HasValue) rawGamerName = IsRawText(gamerName);

            _items.Add(celeb => celeb.CallFunction("ADD_WINNER_TO_WALL", celeb.WallId, winLose, gamerName, crewName, betAmount, true, teamName, !rawGamerName.Value));
        }

        /// <summary>
        /// Add a position item to the wall.
        /// </summary>
        /// <param name="position">The position the player is (0 = dnf).</param>
        /// <param name="label">The label above the position (empty = off)</param>
        /// <param name="rawLabel">Whether the label is rawText (null = automatic).</param>
        public void AddPosition(int position, string label = "CELEB_YOU_FINISHED", bool? rawLabel = null)
        {
            if (!rawLabel.HasValue) rawLabel = IsRawText(label);

            _items.Add(celeb => celeb.CallFunction("ADD_POSITION_TO_WALL", celeb.WallId, position < 1 ? "dnf" : position.ToString(), label, rawLabel.Value, false));
        }

        /// <summary>
        /// Add a score item to the wall.
        /// </summary>
        /// <param name="value">The label with the type of score.</param>
        /// <param name="title">The value the player has scored.</param>
        public void AddScore(int value, string title = "CELEB_SCORE")
        {
            _items.Add(celeb => celeb.CallFunction("ADD_SCORE_TO_WALL", celeb.WallId, title, value));
        }

        /// <summary>
        /// Add a RP and Rank item to the wall.
        /// </summary>
        /// <param name="pointsGained">The amount of points the player gained.</param>
        /// <param name="startPoints">The amount of points the player has a the start.</param>
        /// <param name="rankMinPoints">The amount of points the rank starts at.</param>
        /// <param name="rankMaxPoints">The amount of points needed to rank up.</param>
        /// <param name="currentRank">The the current rank of the player.</param>
        /// <param name="nextRank">The rank after the current rank.</param>
        /// <param name="currentRankName">The name of the current rank (empty = off).</param>
        /// <param name="nextRankName">The nam of the rank after the current rank (empty = off).</param>
        public void AddRepPointsAndRankBar(int pointsGained, int startPoints, int rankMinPoints, int rankMaxPoints, int currentRank, int nextRank, string currentRankName = "", string nextRankName = "")
        {
            _items.Add(celeb => celeb.CallFunction("ADD_REP_POINTS_AND_RANK_BAR_TO_WALL", celeb.WallId, pointsGained, startPoints, rankMinPoints, rankMaxPoints, currentRank, nextRank, currentRankName, nextRankName));
        }

        /// <summary>
        /// Add a Arena Points and Arena Rank item to the wall.
        /// </summary>
        /// <param name="pointsGained">The amount of points the player gained.</param>
        /// <param name="startPoints">The amount of points the player has a the start.</param>
        /// <param name="rankMinPoints">The amount of points the rank starts at.</param>
        /// <param name="rankMaxPoints">The amount of points needed to rank up.</param>
        /// <param name="currentRank">The the current rank of the player.</param>
        /// <param name="nextRank">The rank after the current rank.</param>
        /// <param name="currentRankName">The name of the current rank (empty = off).</param>
        /// <param name="nextRankName">The nam of the rank after the current rank (empty = off).</param>
        public void AddArenaPointsAndRankBar(int pointsGained, int startPoints, int rankMinPoints, int rankMaxPoints, int currentRank, int nextRank, string currentRankName = "", string nextRankName = "")
        {
            _items.Add(celeb => celeb.CallFunction("ADD_ARENA_POINTS_AND_RANK_BAR_TO_WALL", celeb.WallId, pointsGained, startPoints, rankMinPoints, rankMaxPoints, currentRank, nextRank, currentRankName, nextRankName));
        }

        /// <summary>
        /// Add a job points item to the wall.
        /// </summary>
        /// <param name="points">The amount of points.</param>
        public void AddJobPoint(int points)
        {
            _items.Add(celeb => celeb.CallFunction("ADD_JOB_POINTS_TO_WALL", celeb.WallId, points, 0));
        }

        /// <summary>
        /// Add a arena points item to the wall.
        /// </summary>
        /// <param name="points">The amount of arena points.</param>
        public void AddArenaPoint(int points)
        {
            _items.Add(celeb => celeb.CallFunction("ADD_ARENA_POINTS_TO_WALL", celeb.WallId, points, 0));
        }

        /// <summary>
        /// Add a mission result item to the wall.
        /// </summary>
        /// <param name="passed">Whether the player has passed the mission.</param>
        /// <param name="mission">The name of the mission.</param>
        /// <param name="missionReason">The reason why the mission has ended.</param>
        /// <param name="rawMission">Whether the mission name is rawText (null = automatic).</param>
        /// <param name="rawMissionReason">Whether the mission reason is rawText (null = automatic).</param>
        public void AddMissionResult(bool passed, string mission = "", string missionReason = "", bool? rawMission = null, bool? rawMissionReason = null)
        {
            AddMissionResult(mission, passed ? "CELEB_PASSED" : "CELEB_FAILED", missionReason, rawMission, rawMissionReason, false);
        }

        /// <summary>
        /// Add a mission result item to the wall.
        /// </summary>
        /// <param name="mission">The name of the mission.</param>
        /// <param name="passFail">The pass or fail test.</param>
        /// <param name="missionReason">The reason why the mission has ended.</param>
        /// <param name="rawMission">Whether the mission name is rawText (null = automatic).</param>
        /// <param name="rawPassFail">Whether the pass fail message is rawText (null = automatic).</param>
        /// <param name="rawMissionReason">Whether the mission reason is rawText (null = automatic).</param>
        public void AddMissionResult(string mission = "", string passFail = "", string missionReason = "", bool? rawMission = null, bool? rawPassFail = null, bool? rawMissionReason = null)
        {
            if (!rawMission.HasValue) rawMission = IsRawText(mission);
            if (!rawPassFail.HasValue) rawPassFail = IsRawText(passFail);
            if (!rawMissionReason.HasValue) rawMissionReason = IsRawText(missionReason);

            _items.Add(celeb => celeb.CallFunction("ADD_MISSION_RESULT_TO_WALL", celeb.WallId, mission, passFail, missionReason, rawMissionReason.Value, rawPassFail.Value, rawMission.Value));
        }

        /// <summary>
        /// Add a time item to the wall.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="timeDiff">The difference with the time (0 = off).</param>
        /// <param name="label">The label of the item.</param>
        public void AddTime(int time, int timeDiff = 0, string label = "CELEB_TIME")
        {
            _items.Add(celeb => celeb.CallFunction("ADD_TIME_TO_WALL", celeb.WallId, time, label, timeDiff));
        }

        /// <summary>
        /// Add a cash item to the wall.
        /// </summary>
        /// <param name="cash">The amount of cash.</param>
        public void AddCash(int cash)
        {
            _items.Add(celeb => celeb.CallFunction("ADD_CASH_TO_WALL", celeb.WallId, cash, 0));
        }

        /// <summary>
        /// Add a world record item to the wall.
        /// </summary>
        /// <param name="time">The time of the record.</param>
        public void AddWorldRecord(int time)
        {
            _items.Add(celeb => celeb.CallFunction("ADD_WORLD_RECORD_TO_WALL", celeb.WallId, time));
        }

        /// <summary>
        /// Add a tournament item to the wall.
        /// </summary>
        /// <param name="playlistName">The name of the playlist.</param>
        /// <param name="resultText">The name of the tournament.</param>
        /// <param name="resultValue">The value of the tournament.</param>
        /// <param name="qualification">The label with the message.</param>
        /// <param name="rawResult">Whether the result is rawText (null = automatic).</param>
        public void AddTournament(string playlistName, string resultText, string resultValue, string qualification = "CELEB_QUALIFICATION_COMPLETE", bool? rawResult = null)
        {
            if (!rawResult.HasValue) rawResult = IsRawText(resultText);

            _items.Add(celeb => celeb.CallFunction("ADD_TOURNAMENT_TO_WALL", celeb.WallId, playlistName, qualification, resultText, rawResult.Value, resultValue));
        }

        /// <summary>
        /// Add a objective item to the wall.
        /// </summary>
        /// <param name="title">The title of the objective.</param>
        /// <param name="text">The text of the objective.</param>
        /// <param name="rawTitle">Whether the title is rawText (null = automatic).</param>
        public void AddObjective(string title, string text, bool? rawTitle = null)
        {
            if (!rawTitle.HasValue) rawTitle = IsRawText(title);

            _items.Add(celeb => celeb.CallFunction("ADD_OBJECTIVE_TO_WALL", celeb.WallId, title, text, rawTitle.Value));
        }

        /// <summary>
        /// Add a numeric stat item to the wall.
        /// </summary>
        /// <param name="stat">The name of the stat.</param>
        /// <param name="value">The value of the stat</param>
        /// <param name="rawStat">Whether the stat is rawText (null = automatic).</param>
        public void AddStatNumeric(string stat, int value, bool? rawStat = null)
        {
            if (!rawStat.HasValue) rawStat = IsRawText(stat);

            _items.Add(celeb => celeb.CallFunction("ADD_STAT_NUMERIC_TO_WALL", celeb.WallId, stat, value, 0, rawStat.Value));
        }

        /// <summary>
        /// Add a wave reached item to the wall.
        /// </summary>
        /// <param name="value">The wave that has been reached.</param>
        /// <param name="title">The label with the title that has been reached.</param>
        public void AddWaveReached(int value, string title = "BM_WAVE_COMP")
        {
            _items.Add(celeb => celeb.CallFunction("ADD_WAVE_REACHED_TO_WALL", celeb.WallId, value, title));
        }

        /// <summary>
        /// Add a post unlock cash item to the wall.
        /// </summary>
        /// <param name="cash">The amount of cash</param>
        public void AddPostUnlockCash(int cash)
        {
            _items.Add(celeb => celeb.CallFunction("ADD_POST_UNLOCK_CASH_TO_WALL", celeb.WallId, cash, 0));
        }

        /// <summary>
        /// Add a challenge winner item to the wall.
        /// </summary>
        /// <param name="challenge">The name of the challenge.</param>
        /// <param name="winner">The winner of the challenge.</param>
        /// <param name="isMission">Whether the challenge is a mission.</param>
        /// <param name="hasWon">Whether the player has won the challenge.</param>
        /// <param name="crew">The name of the crew that has won the challenge.</param>
        /// <param name="bet">The bet amount the player a gained/lost (0 = off).</param>
        public void AddChallengeWinner(string challenge, string winner, bool isMission, bool hasWon, string crew = "", int bet = 0)
        {
            AddChallengeWinner(challenge, winner, isMission, hasWon ? "CELEB_WINNER" : "CELEB_LOSER", crew, bet, false);
        }

        /// <summary>
        /// Add a challenge winner item to the wall.
        /// </summary>
        /// <param name="challenge">The name of the challenge.</param>
        /// <param name="winner">The winner of the challenge.</param>
        /// <param name="isMission">Whether the challenge is a mission.</param>
        /// <param name="winLose">The winLose text.</param>
        /// <param name="crew">The name of the crew that has won the challenge.</param>
        /// <param name="bet">The bet amount the player a gained/lost (0 = off).</param>
        /// <param name="rawWinLose">Whether the win lose text is rawText (null = automatic).</param>
        public void AddChallengeWinner(string challenge, string winner, bool isMission, string winLose = "CELEB_WINNER", string crew = "", int bet = 0, bool? rawWinLose = null)
        {
            if (!rawWinLose.HasValue) rawWinLose = IsRawText(winLose);

            _items.Add(celeb => celeb.CallFunction("ADD_CHALLENGE_WINNER_TO_WALL", celeb.WallId, challenge, winLose, crew, winner, bet, true, true, isMission, rawWinLose.Value, ""));
        }

        /// <summary>
        /// Check whether text is rawText or a label.
        /// </summary>
        /// <param name="text">The text to be checked.</param>
        /// <returns>Whether the text is rawText.</returns>
        public bool IsRawText(string text)
        {
#if FIVEM
            return !API.DoesTextLabelExist(text);
#elif RPH
            return NativeFunction.CallByHash<bool>(0xAC09CA973C564252, text);
#elif (SHVDN2 || SHVDN3)
            return Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, text);
#endif
        }
    }

    /// <summary>
    /// The layer for a CelebrationPart.
    /// </summary>
    public enum CelebrationLayerEnum
    {
        Main,
        Background,
        Foreground,
    }

    /// <summary>
    /// Extension methods for the CelebrationLayer enum.
    /// </summary>
    public static class CelebrationLayerExtensions
    {
        /// <summary>
        /// Get the name of the Scaleform for a CelebrationLayer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns>The scaleform for the layer.</returns>
        public static string Scaleform(this CelebrationLayerEnum layer)
        {
            switch (layer)
            {
                case CelebrationLayerEnum.Background:
                    return "MP_CELEBRATION_BG";
                case CelebrationLayerEnum.Foreground:
                    return "MP_CELEBRATION_FG";
            }

            return "MP_CELEBRATION";
        }

        /// <summary>
        /// Get the colour of the wall for a CelebrationLayer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns>The HUD colour for the layer.</returns>
        public static string WallColour(this CelebrationLayerEnum layer)
        {
            switch (layer)
            {
                case CelebrationLayerEnum.Background:
                    return "HUD_COLOUR_BLACK";
                case CelebrationLayerEnum.Foreground:
                    return "HUD_COLOUR_RED";
            }

            return "HUD_COLOUR_BLUE";
        }
    }
}
