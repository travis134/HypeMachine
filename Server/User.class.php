<?php

require_once 'Db_Singleton.class.php'; 
require_once 'Db_Object.interface.php';
require_once 'Xml_Serializable_Object.class.php'; 

final class User extends Xml_Serializable_Object implements Db_Object
{
	protected $id;
	protected $anid;
	protected $nickname;
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
				else if(isset($args['anid']))
				{
					$this->setAnid($args['anid']);
				}
				else if(isset($args['nickname']))
				{
					$this->setNickname($args['nickname']);
				}
				break;
			case 2:
				if(isset($args['anid']))
				{
					$this->setAnid($args['anid']);
				}
				if(isset($args['nickname']))
				{
					$this->setNickname($args['nickname']);
				}
				$this->create();
				break;
			case 3:
				if(isset($args['id']))
				{
					$this->setId($args['id']);
				}
				if(isset($args['anid']))
				{
					$this->setAnid($args['anid']);
				}
				if(isset($args['nickname']))
				{
					$this->setNickname($args['nickname']);
				}
				break;
		}
		$this->setExists($this->read());
	}
	
	public function __destruct()
	{
		$this->id = null;
		$this->anid = null;
		$this->nickname = null;
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
	
	public function setAnid($anid)
	{
		$this->anid = substr($anid, 0, min(32, strlen($anid)));
	}
	
	public function getAnid()
	{
		return $this->anid;
	}
	
	public function setNickname($nickname)
	{
		$this->nickname = substr($nickname, 0, min(12, strlen($nickname)));
	}
	
	public function getNickname()
	{
		return $this->nickname;
	}
	
	public static function readAll()
	{
		$results = array();
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("SELECT * FROM user_table");
		$flag = $sth->execute();
		
		if($flag)
		{
			$sth->setFetchMode(PDO::FETCH_ASSOC);
			
			while($row = $sth->fetch())
			{
				$results[] =  new User($row);
			}
		}
		
		return $results;
	}
	
	public function create()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("INSERT INTO user_table (anid, nickname) VALUES (:anid, :nickname)");
		$flag = $sth->execute(array('anid' => $this->getAnid(), 'nickname' => $this->getNickname()));
		
		if($flag)
		{
			$this->setId($dbh->lastInsertId());
		}
		
		return $flag;
	}
	
	public function read()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("SELECT * FROM user_table WHERE (id = :id) OR (anid = :anid) OR (nickname = :nickname) LIMIT 0, 1");
		$flag = $sth->execute(array('id' => $this->getId(), 'anid' => $this->getAnid(), 'nickname' => $this->getNickname()));
		
		if($flag)
		{
			$sth->setFetchMode(PDO::FETCH_ASSOC);
			$row = $sth->fetch();
			
			if($row)
			{
				$this->setId($row['id']);
				$this->setAnid($row['anid']);
				$this->setNickname($row['nickname']);
			}
		}
		
		return $row;
	}
	
	public function update()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("UPDATE user_table SET anid = :anid, nickname = :nickname WHERE id = :id");
		$flag = $sth->execute(array('id' => $this->getId(), 'anid' => $this->getAnid(), 'nickname' => $this->getNickname()));
		
		return $flag;
	}
	
	public function delete()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("DELETE FROM user_table WHERE id = :id");
		$flag = $sth->execute(array('id' => $this->getId()));
		
		return $flag;
	}
}

?>