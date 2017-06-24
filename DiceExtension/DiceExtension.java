package sfs2x.extensions.games.dice;

//import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class DiceExtension extends SFSExtension
{
	private User whoseTurn;
	private Integer maxPlayers = null;
	private volatile boolean gameStarted;
	
	@Override
	public void init()
	{	
		this.addRequestHandler("ready", ReadyHandler.class);
		this.addRequestHandler("sendRoll", RollHandler.class);
		this.addRequestHandler("sendChipData", ChipHandler.class);
	}
	
	Integer getMaxPlayers()
	{
		return maxPlayers;
	}
	
	void setMaxPlayers(int players)
	{
		maxPlayers = players;
	}
	
	boolean canStartGame()
	{
		return this.getGameRoom().getSize().getUserCount() == maxPlayers;
	}
	
	Room getGameRoom()
	{
		return this.getParentRoom();
	}
	
	int updateTurn()
	{
		int newTurn = whoseTurn.getPlayerId() + 1;
		
		if (newTurn > maxPlayers)
			newTurn = 1;
		
		whoseTurn = getParentRoom().getUserByPlayerId(newTurn);
		return newTurn;
	}
	
	User getWhoseTurn()
    {
	    return whoseTurn;
    }
	
	void startGame()
	{
		trace("extension start function called");
		
		if (gameStarted)
			throw new IllegalStateException("Game is already started!");
		
		gameStarted = true;
		
		User player1 = getParentRoom().getUserByPlayerId(1);
		//User player2 = getParentRoom().getUserByPlayerId(2);
		
		// No turn assigned? Let's start with player 1
		if (whoseTurn == null)
			whoseTurn = player1;
		
		trace("whose turn: " + whoseTurn.getPlayerId());
		
		// Send START event to client
		ISFSObject resObj = new SFSObject();
		resObj.putInt("t", whoseTurn.getPlayerId());
		//resObj.putUtfString("p1n", player1.getName());
		//resObj.putInt("p1i", player1.getId());
		//resObj.putUtfString("p2n", player2.getName());
		//resObj.putInt("p2i", player2.getId());
		
		send("start", resObj, getGameRoom().getUserList());
	}

}
