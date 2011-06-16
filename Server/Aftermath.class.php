<?php

require_once 'Rating.class.php';

final class Aftermath extends Rating
{
	public function __construct($args)
	{
		parent::__construct($args);
		if(sizeof($args) == 3)
		{
			$this->create();
		}
		$this->setExists($this->read());
	}
	
	public function __destruct()
	{
		parent::__destruct($args);
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
		else if(isset($args['guid']))
		{
			$arg_name = 'game_id';
			$temp_game = new Game(array('guid' => $args['guid']));
			$arg_value = $temp_game->getId();
		}
		else if(isset($args['anid']))
		{
			$arg_name = 'user_id';
			$temp_user = new User(array('anid' => $args['anid']));
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
			$sth = $dbh->prepare("SELECT * FROM aftermath_table");
			$flag = $sth->execute();
		}
		else
		{
			$sth = $dbh->prepare("SELECT * FROM aftermath_table WHERE $arg_name = :$arg_name");
			$flag = $sth->execute(array( $arg_name => $arg_value));
		}
		
		if($flag)
		{
			$sth->setFetchMode(PDO::FETCH_ASSOC);
			
			while($row = $sth->fetch())
			{
				$results[] =  new Aftermath($row);
			}
		}
		
		return $results;
	}
	
	public function create()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("INSERT INTO aftermath_table (game_id, user_id, score) VALUES (:game_id, :user_id, :score)");
		$flag = $sth->execute(array('game_id' => $this->getGameId(), 'user_id' => $this->getUserId(), 'score' => $this->getScore()));
		
		if($flag)
		{
			$this->setId($dbh->lastInsertId());
		}
		
		return $flag;
	}
	
	public function read()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("SELECT * FROM aftermath_table WHERE (id = :id) OR (game_id = :game_id AND user_id = :user_id) LIMIT 0, 1");
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
				$this->setScore($row['score']);
			}
		}
		
		return $row;
	}
	
	public function update()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("UPDATE aftermath_table SET game_id = :game_id, user_id = :user_id, score = :score WHERE id = :id");
		$flag = $sth->execute(array('id' => $this->getId(), 'game_id' => $this->getGameId(), 'user_id' => $this->getUserId(), 'score' => $this->getScore()));

		return $flag;
	}
	
	public function delete()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("DELETE FROM aftermath_table WHERE id = :id");
		$flag = $sth->execute(array('id' => $this->getId()));
		
		return $flag;
	}
}

?>