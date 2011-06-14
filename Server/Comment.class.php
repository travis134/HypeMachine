<?php

require_once 'Db_Singleton.class.php'; 
require_once 'Db_Object.interface.php';
require_once 'Xml_Serializable_Object.class.php'; 
require_once 'User.class.php';
require_once 'Game.class.php';

final class Comment extends Xml_Serializable_Object implements Db_Object
{
	protected $id;
	protected $game_id;
	protected $user_id;
	protected $content;
	protected $date;
	private $exists;

	public function __construct($args)
	{
		$numArgs = sizeof($args);
		switch($numArgs)
		{
			case 0:
				break;
			case 1:
				if(isset($args['id']))
				{
					$this->setId($args['id']);
				}
				break;
			case 2:
				if(isset($args['game_id']))
				{
					$this->setGameId($args['game_id']);
				}
				if(isset($args['user_id']))
				{
					$this->setUserId($args['user_id']);
				}
				break;
			case 3:
				if(isset($args['game_id']))
				{
					$this->setGameId($args['game_id']);
				}
				if(isset($args['user_id']))
				{
					$this->setUserId($args['user_id']);
				}
				if(isset($args['content']))
				{
					$this->setContent($args['content']);
				}
				$this->setDate(time());
				$this->create();
				break;
			case 4:
				if(isset($args['game_id']))
				{
					$this->setGameId($args['game_id']);
				}
				if(isset($args['user_id']))
				{
					$this->setUserId($args['user_id']);
				}
				if(isset($args['content']))
				{
					$this->setContent($args['content']);
				}
				if(isset($args['date']))
				{
					$this->setDate($args['date']);
				}
				$this->create();
				break;
			case 5:
				if(isset($args['id']))
				{
					$this->setId($args['id']);
				}
				if(isset($args['game_id']))
				{
					$this->setGameId($args['game_id']);
				}
				if(isset($args['user_id']))
				{
					$this->setUserId($args['user_id']);
				}
				if(isset($args['content']))
				{
					$this->setContent($args['content']);
				}
				if(isset($args['date']))
				{
					$this->setDate($args['date']);
				}
				break;
		}
		$this->setExists($this->read());
	}
	
	public function __destruct()
	{
		$this->id = null;
		$this->game_id = null;
		$this->user_id = null;
		$this->content = null;
		$this->date = null;
	}
	
	public function getExists()
	{
		return $this->exists;
	}

	public function setExists($exists)
	{
		$this->exists = $exists;;
	}
	
	public function setId($id)
	{
		$this->id = abs(intval($id));
	}
	
	public function getId()
	{
		return $this->id;
	}
	
	public function setGameId($game_id)
	{
		$this->game_id = abs(intval($game_id));
	}
	
	public function getGameId()
	{
		return $this->game_id;
	}
	
	public function setUserId($user_id)
	{
		$this->user_id = abs(intval($user_id));
	}
	
	public function getUserId()
	{
		return $this->user_id;
	}
	
	public function setContent($content)
	{
		$this->content = htmlentities($content);
	}
	
	public function getContent()
	{
		return $this->content;
	}
	
	public function setDate($date)
	{
		if(!is_int($date))
		{
			$date = strtotime($date);
		}
		$this->date = date('Y-m-d H:i:s', $date);
	}
	
	public function getDate()
	{
		return $this->date;
	}
	
	public static function readAllWithValue($args)
	{
		$arg_name = null;
		$arg_value = null;
	
		if(isset($args['game_id']))
		{
			$arg_name = 'game_id';
			$arg_value = $args['game_id'];
		}
		else if(isset($args['user_id']))
		{
			$arg_name = 'user_id';
			$arg_value = $args['user_id'];
		}
		else if(isset($args['date']))
		{
			$arg_name = 'date';
			$arg_value = $args['date'];
		}
		else if(isset($args['guid']))
		{
			$arg_name = 'game_id';
			$temp_game = new Game(array('guid' => $args['guid']));
			$arg_value = $temp_game->getId();
		}
		else if(isset($args['device_unique_id']))
		{
			$arg_name = 'user_id';
			$temp_user = new User(array('device_unique_id' => $args['device_unique_id']));
			$arg_value = $temp_user->getId();
		}
		else if(isset($args['nickname']))
		{
			$arg_name = 'user_id';
			$temp_user = new User(array('nickname' => $args['nickname']));
			$arg_value = $temp_user->getId();
		}
		
		$results = array();
		$dbh = Db_Singleton::instance();
		if(is_null($arg_name) && is_null($arg_value))
		{
			$sth = $dbh->prepare("SELECT * FROM comment_table");
			$flag = $sth->execute();
		}
		else
		{
			$sth = $dbh->prepare("SELECT * FROM comment_table WHERE $arg_name = :$arg_name");
			$flag = $sth->execute(array( $arg_name => $arg_value));
		}
		
		if($flag)
		{
			$sth->setFetchMode(PDO::FETCH_ASSOC);
			
			while($row = $sth->fetch())
			{
				$results[] =  new Comment($row);
			}
		}
		
		return $results;
	}
	
	public function create()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("INSERT INTO comment_table (game_id, user_id, content, date) VALUES (:game_id, :user_id, :content, :date)");
		$flag = $sth->execute(array('game_id' => $this->getGameId(), 'user_id' => $this->getUserId(), 'content' => $this->getContent(), 'date' => $this->getDate()));
		
		if($flag)
		{
			$this->setId($dbh->lastInsertId());
		}
		
		return $flag;
	}
	
	public function read()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("SELECT * FROM comment_table WHERE (id = :id) OR (game_id = :game_id AND user_id = :user_id) LIMIT 0, 1");
		$flag = $sth->execute(array('id' => $this->getId(), 'game_id' => $this->getGameId(), 'user_id' => $this->getUserId()));
		
		if($flag)
		{
			$sth->setFetchMode(PDO::FETCH_ASSOC);
			$row = $sth->fetch();
			
			if($row)
			{
				$this->setId($row['id']);
				$this->setGameId($row['game_id']);
				$this->setUserId($row['user_id']);
				$this->setContent($row['content']);
				$this->setDate($row['date']);
			}
		}
		
		return $row;
	}
	
	public function update()
	{
		$this->setDate(time());
	
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("UPDATE comment_table SET game_id = :game_id, user_id = :user_id, content = :content, date = :date WHERE id = :id");
		$flag = $sth->execute(array('id' => $this->getId(), 'game_id' => $this->getGameId(), 'user_id' => $this->getUserId(), 'content' => $this->getContent(), 'date' => $this->getDate()));
		
		return $flag;
	}
	
	public function delete()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("DELETE FROM comment_table WHERE id = :id");
		$flag = $sth->execute(array('id' => $this->getId()));
		
		return $flag;
	}
}

?>