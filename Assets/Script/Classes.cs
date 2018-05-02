using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region data_to_send

//friend
[Serializable]
public class check_ver
{
    [SerializeField]
    private string version;


    public void setVer(string id)
    {
        version = id;
    }

    public string getVer()
    {
        return this.version;
    }

}

//friend
[Serializable]
public class send_emoji
{
    [SerializeField]
    private int player_id;
    [SerializeField]
    private int emoji;


    public void setPlayerId(int id)
    {
        player_id = id;
    }

    public int getPlayerId()
    {
        return this.player_id;
    }

    public void setEmoji(int id)
    {
        emoji = id;
    }

    public int getEmoji()
    {
        return this.emoji;
    }
}

//friend
[Serializable]
public class add_friend
{
    [SerializeField]
    private int player_id;
    [SerializeField]
    private string friend_username; 


    public void setPlayerId(int id)
    {
        player_id = id;
    }

    public int getPlayerId()
    {
        return this.player_id;
    }

    public void setFriendId(string id)
    {
        friend_username = id;
    }

    public string getFriendId()
    {
        return this.friend_username;
    }
}

[Serializable]
public class delete_friend
{
    [SerializeField]
    private int player_id;
    [SerializeField]
    private string access_token;
    [SerializeField]
    private int friend_id;
    [SerializeField]
    private string display_name;

    public void setDisplay(string id)
    {
        display_name = id;
    }

    public string getDisplay()
    {
        return this.display_name;
    }

    public void setAccessToken(string id)
    {
        access_token = id;
    }

    public string getAccessToken()
    {
        return this.access_token;
    }

    public void setPlayerId(int id)
    {
        player_id = id;
    }

    public int getPlayerId()
    {
        return this.player_id;
    }

    public void setFriendId(int id)
    {
        friend_id = id;
    }

    public int getFriendId()
    {
        return this.friend_id;
    }
}
[Serializable]
public class send_user_data
{
    [SerializeField]
    private int player_id;
    [SerializeField]
    private string access_token;
    [SerializeField]
    private int game_id;



    public void setPlayerId(int id)
    {
        player_id = id;
    }

    public int getPlayerId()
    {
        return this.player_id;
    }
    public void setGameId(int id)
    {
        game_id = id;
    }

    public int getGameId()
    {
        return this.game_id;
    }

    public void setAccessToken(string data)
    {
        access_token = data;
    }

    public string getAccessToken()
    {
        return this.access_token;
    }
}

//avatar
[Serializable]
public class avatar_data
{
    [SerializeField]
    private int player_id;
    [SerializeField]
    private int avatar_id;


    public void setPlayerId(int id)
    {
        player_id = id;
    }

    public int getPlayerId()
    {
        return this.player_id;
    }

    public void setAvatarId(int id)
    {
        avatar_id = id;
    }

    public int getAvatarId()
    {
        return this.avatar_id;
    }
}
//login 
[Serializable]
public class create_oa
{
    [SerializeField]
    private string username;
    [SerializeField]
    private string password;
    [SerializeField]
    private int player_id;
    [SerializeField]
    private int game_id;
    [SerializeField]
    private int method;

    public void setUsername(string name)
    {
        username = name;
    }

    public string getUsername()
    {
        return this.username;
    }
    public void setPlayerId(int data)
    {
        player_id = data;
    }

    public void setGameId(int data)
    {
        game_id = data;
    }

    public void setMethod(int data)
    {
        method = data;
    }
    public void setPassword(string pass)
    {
        password = pass;
    }

    public string getPassword()
    {
        return this.password;
    }


}
//login 
[Serializable]
public class login_user
{
    [SerializeField]
    private string username;
    [SerializeField]
    private string password;
    [SerializeField]
    private string ip_address;
    [SerializeField]
    private string country;

    public void setUsername(string name)
    {
        username = name;
    }

    public string getUsername()
    {
        return this.username;
    }
    public void setIp(string data)
    {
        ip_address = data;
    }

    public string getCountry()
    {
        return this.country;
    }
    public void setCountry(string data)
    {
        country = data;
    }

    public string getIp()
    {
        return this.ip_address;
    }
    public void setPassword(string pass)
    {
        password = pass;
    }

    public string getPassword()
    {
        return this.password;
    }

   
}
//fb
[Serializable]
public class facebook_user
{
    [SerializeField]
    private string facebook_id;
    [SerializeField]
    private string country;
    [SerializeField]
    private string ip_address;
    [SerializeField]
    private int method;
    [SerializeField]
    private int game_id;
    [SerializeField]
    private string display_name;

    public void setDisplayName(string data)
    {
        display_name = data;
    }

    public string getDisplayName()
    {
        return this.display_name;
    }

    public void setUsername(string data)
    {
        facebook_id = data;
    }

    public string getUsername()
    {
        return this.facebook_id;
    }

    public void setCountry(string data)
    {
        country = data;
    }

    public string getCountry()
    {
        return this.country;
    }
    public void setIp(string data)
    {
        ip_address = data;
    }

    public string getIp()
    {
        return this.ip_address;
    }

    public void setMethod(int data)
    {
        method = data;
    }

    public int getMethod()
    {
        return this.method;
    }

    public void setGame_Id(int data)
    {
        game_id = data;
    }

    public int getGame_Id()
    {
        return this.game_id;
    }

}
//guest
[Serializable]
public class guest_user
{
    [SerializeField]
    private string country;
    [SerializeField]
    private string ip_address;
    [SerializeField]
    private int method;
    [SerializeField]
    private int game_id;
    [SerializeField]
    private int character;


    public void setCountry(string data)
    {
        country = data;
    }

    public string getCountry()
    {
        return this.country;
    }
    public void setIp(string data)
    {
        ip_address = data;
    }

    public string getIp()
    {
        return this.ip_address;
    }

    public void setMethod(int data)
    {
        method = data;
    }

    public int getMethod()
    {
        return this.method;
    }

    public void setGame_Id(int data)
    {
        game_id = data;
    }

    public int getGame_Id()
    {
        return this.game_id;
    }

    public void setCharacter(int data)
    {
        character = data;
    }

    public int getCharacter()
    {
        return this.character;
    }

}

//match making
[Serializable]
public class match_making_data
{
    [SerializeField]
    private int player_id;
    [SerializeField]
    private string access_token;
    [SerializeField]
    private int character;
    
    
    public int getPlayerID()
    {
        return this.player_id;
    }
    public void setPlayerID(int data)
    {
        this.player_id = data;
    }
    public string getAccessToken()
    {
        return this.access_token;
    }
    public void setAccessToken(string data)
    { 
        this.access_token = data;
    }
    public int getCharacter()
    {
        return this.character;
    }
    public void setCharacter(int data)
    {
        this.character = data;
    }

}

//timer
[Serializable]
public class timer_data
{
    public int player_id;
    public int match_id;
}
//direction
[Serializable]
public class direction_data
{
    public int player_id;
    public int match_id;
    public int direction;
}
//striker 
[Serializable]
public class striker_data
{
    public int player_id;
    public int match_id;
    public int striker_id;
}
//keeper
[Serializable]
public class keeper_data
{
    public int player_id;
    public int match_id;
    public int keeper_id;
}
#endregion

#region data_to_receive
//friend   
[Serializable]
public class root_friend_data
{
    public friend_data data;
    public error_data error;
}
[Serializable]
public struct friend_list_data
{
    public int avatar_id;
    public int id;
    public string display_name;
    public string status;
    public int rank;
    public int win_rate;
}
[Serializable]
public class friend_data
{
    public friend_list_data[] friend_list;
    
}

//login from other device

[Serializable]
public class root_login_from_device
{
    public error_data error;
}

//leaderboard   
[Serializable]
public class root_leaderboard_data
{
    public leaderboard_data data;
    public error_data error;
}
[Serializable]
public struct leaderboard_list_data
{
    public int avatar_id;
    public string display_name;
    public int win_rate;
}
[Serializable]
public class leaderboard_data
{
    public leaderboard_list_data[] list;
}

//Login 
[Serializable]
public class player_data
{
    public user_data data;
    public error_data error;
}
[Serializable]
public class user_data
{
    [SerializeField]
    private int player_id;
    [SerializeField]
    private string access_token;



    public void setPlayerId(int id)
    {
        player_id = id;
    }

    public int getPlayerId()
    {
        return this.player_id;
    }

    public void setAccessToken(string data)
    {
        access_token = data;
    }

    public string getAccessToken()
    {
        return this.access_token;
    }
}//Login 

[Serializable]
public class root_receive_emoji
{
    public receive_emoji data;
    public error_data error;
}
[Serializable]
public class receive_emoji
{
    [SerializeField]
    private int emoji;



    public void setEmoji(int id)
    {
        emoji = id;
    }

    public int getEmoji()
    {
        return this.emoji;
    }


}


//inviteTimer
[Serializable]
public class root_invite_timer
{
    public invite_timer data;
    public error_data error;
}
[Serializable]
public class invite_timer
{
    [SerializeField]
    private int endpoint;
    [SerializeField]
    private int time_left;
    [SerializeField]
    private string inviter;


    public string getInviter()
    {
        return this.inviter;
    }


    public int getEndPoint()
    {
        return this.endpoint;
    }


    public int getTimeLeft()
    {
        return this.time_left;
    }
}

//Online Acc 
[Serializable]
public class root_oa
{
    public error_data error;
}
//ver Acc 
[Serializable]
public class get_Ver
{
    public error_data error;
}

//Daily Login 
[Serializable]
public class daily_login
{
    public error_data error;
}

//new Balance 
[Serializable]
public class root_new_balance
{
    public new_balance1 data;
    public error_data error;
}
[Serializable]
public class new_balance1
{
    [SerializeField]
    private int new_balance;



    public void setNewBalance(int id)
    {
        new_balance = id;
    }

    public int getNewBalance()
    {
        return this.new_balance;
    }

}


//Menu
[Serializable]
public class menu_data
{
    public menu_user_data data;
    public error_data error;
}
[Serializable]
public class menu_user_data
{
    [SerializeField]
    private int avatar_id;
    [SerializeField]
    private double balance;
    [SerializeField]
    private int rank;
    [SerializeField]
    private string username;
    [SerializeField]
    private string display_name;
    [SerializeField]
    private int method;

    public void setBalance(double data)
    {
        balance = data;
    }

    public double getBalance()
    {
        return this.balance;
    }
    public void setMethod(int data)
    {
        method = data;
    }

    public int getMethod()
    {
        return this.method;
    }
    public void setDisplayName(string data)
    {
        display_name = data;
    }

    public string getDisplayName()
    {
        return this.display_name;
    }

    public void setUsername(string data)
    {
        username = data;
    }

    public string getUsername()
    {
        return this.username;
    }
    public void setAvatar(int data)
    {
        avatar_id = data;
    }

    public int getAvatar()
    {
        return this.avatar_id;
    }
    public void setRank(int data)
    {
        rank = data;
    }

    public int getRank()
    {
        return this.rank;
    }
}

//Profile
[Serializable]
public class statistics_data
{
    public player_statistics data;
    public error_data error;
}
[Serializable]
public class player_statistics
{
    [SerializeField]
    private int current_exp;
    [SerializeField]
    private int win_rate;
    [SerializeField]
    private int total_match;
    [SerializeField]
    private int total_goals;
    [SerializeField]
    private int current_rank;
    [SerializeField]
    private int total_blocks;

    public void setRank(int data)
    {
        current_rank = data;
    }

    public int getRank()
    {
        return this.current_rank;
    }

    public void setExp(int data)
    {
        current_exp = data;
    }

    public int getExp()
    {
        return this.current_exp;
    }
    public void setWinRate(int data)
    {
        win_rate = data;
    }

    public int getWinRate()
    {
        return this.win_rate;
    }
    public void setTotalMatch(int data)
    {
        total_match = data;
    }

    public int getTotalMatch()
    {
        return this.total_match;
    }

    public void setTotalGoal(int data)
    {
        total_goals = data;
    }

    public int getTotalGoal()
    {
        return this.total_goals;
    }
    public void setTotalBlock(int data)
    {
        total_blocks = data;
    }

    public int getTotalBlock()
    {
        return this.total_blocks;
    }
}

//settings API 
[Serializable]
public class root_settings_data
{
    public settings_data data;
    public error_data error;
}
[Serializable]
public class settings_data
{
    [SerializeField]
    private int is_guest;
    [SerializeField]
    private string username;



    public int getIsGuest()
    {
        return this.is_guest;
    }

    public void setUsername(string data)
    {
        username = data;
    }

    public string getUsername()
    {
        return this.username;
    }
}
//General
[Serializable]
public class error_data
{
    [SerializeField]
    private int status_code;
    [SerializeField]
    private string message;

    public void setStatusCode(int data)
    {
        status_code = data;
    }

    public int getStatusCode()
    {
        return this.status_code;
    }

    public void setMessage(string data)
    {
        message = data;
    }

    public string getMessage()
    {
        return this.message;
    }
}


//socket
[Serializable]
public class root_match_data
{
    public error_data error;
    public match_data data;
}
[Serializable]
public class match_data
{
    public int role;
    public string opponent_name;
    public int match_id;
    public int opponent_avatar;
    public int new_balance;
}

//timer
[Serializable]
public class root_timer_data
{
    public error_data error;
    public receive_timer_data data;
}


[Serializable]
public class receive_timer_data
{
    public int time_left;
}

//opponent data
[Serializable]
public class root_opponent_data
{
    public error_data error;
    public opponent_data data;
}

[Serializable]
public class opponent_data
{
    public int role;
    public int opponent_keeper;
    public int opponent_striker;
}


//round data
[Serializable]
public class root_round_data
{
    public error_data error;
    public round_data data;
}

[Serializable]
public class round_data
{
    public int role;
    public score_data score;
    public int opponent_direction;
    public score_data opponent_score;
    public int round_result;
    public int result;
    public int saved;
    public int miss;
}
[Serializable]
public class score_data
{
    public score_array[] round;
    public int total;
}

[Serializable]
public struct score_array
{
    public int round_score;
}

#endregion

#region localization_data
[Serializable]
public class LocalizationData
{
    public LocalizationItem[] items;
}

[Serializable]
public class LocalizationItem
{
    public string key;
    public string value;
}
#endregion



//Enumerations
public enum Role { Keeper, Striker };
public enum Direction { Centre, TopLeft, TopRight, BottomLeft, BottomRight };
public enum Menu { match, friends, profile, setting, store };
public enum DynamicPanel { error, confirm, add_friend };
public enum MatchType { novice = 1, amateur, medium, pro, world_class };
public enum Rank { bronze, silver, gold };
public enum Striker { CRonaldo = 1, JHenderson, LMessi, NJr, PPogba, WRooney };
public enum Keeper { David = 1, JHart, GBuffon };
public enum Language { English , Chinese };
public enum FriendPanel { Name, Avatar, Status, Winrate, Rank };
public enum Status { Online, Offline };