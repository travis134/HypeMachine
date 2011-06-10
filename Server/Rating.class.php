<?php

require_once 'Db_Singleton.class.php'; 
require_once 'Db_Object.interface.php';
require_once 'Xml_Serializable_Object.class.php'; 
require_once 'User.class.php';
require_once 'Game.class.php';

abstract class Rating extends Xml_Serializable_Object implements Db_Object
{
	protected $id;
	protected $game_id;
	protected $user_id;
	protected $score;

	protected function __construct($args)
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
				if(isset($args['score']))
				{
					$this->setScore($args['score']);
				}
				break;
			case 4:
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
				if(isset($args['score']))
				{
					$this->setScore($args['score']);
				}
				break;
		}
	}
	
	protected function __destruct()
	{
		$this->id = null;
		$this->game_id = null;
		$this->user_id = null;
		$this->score = null;
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
	
	public function setScore($score)
	{
		$this->score = $score;
	}
	
	public function getScore()
	{
		return $this->score;
	}
}

?>