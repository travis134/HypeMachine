<?php

require_once 'Db_Singleton.class.php'; 
require_once 'Db_Object.interface.php';
require_once 'Xml_Serializable_Object.class.php'; 

final class Game extends Xml_Serializable_Object implements Db_Object
{
	protected $id;
	protected $guid;
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
				else if(isset($args['guid']))
				{
					$this->setGuid($args['guid']);
					$this->create();
				}
				break;
			case 2:
				if(isset($args['id']))
				{
					$this->setId($args['id']);
				}
				if(isset($args['guid']))
				{
					$this->setGuid($args['guid']);
				}
				break;
		}
		$this->setExists($this->read());
	}
	
	public function __destruct()
	{
		$this->id = null;
		$this->guid = null;
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
	
	public function setGuid($guid)
	{
		$this->guid = substr($guid, 0, min(20, strlen($guid)));
	}
	
	public function getGuid()
	{
		return $this->guid;
	}
	
	public static function readAll()
	{
		$results = array();
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("SELECT * FROM game_table");
		$flag = $sth->execute();
		
		if($flag)
		{
			$sth->setFetchMode(PDO::FETCH_ASSOC);
			
			while($row = $sth->fetch())
			{
				$results[] =  new Game($row);
			}
		}
		
		return $results;
	}
	
	public static function readAllWithGuids($guids)
	{
		$results = array();
		$dbh = Db_Singleton::instance();
		
		for($i = 0; $i<count($guids); $i++)
		{
			$temp = new Game(array('guid' => $guids[$i]));
			if($temp->getExists())
			{
				$results[] = $temp;
			}
		}
		
		return $results;
	}
	
	public function create()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("INSERT INTO game_table (guid) VALUES (:guid)");
		$flag = $sth->execute(array('guid' => $this->getGuid()));
		
		if($flag)
		{
			$this->setId($dbh->lastInsertId());
		}
		
		return $flag;
	}
	
	public function read()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("SELECT * FROM game_table WHERE (id = :id) OR (guid = :guid) LIMIT 0, 1");
		$flag = $sth->execute(array('id' => $this->getId(), 'guid' => $this->getGuid()));

		if($flag)
		{
			$sth->setFetchMode(PDO::FETCH_ASSOC);
			$row = $sth->fetch();
			
			if($row)
			{
				$this->setId($row['id']);
				$this->setGuid($row['guid']);
			}
		}
		
		return $row;
	}
	
	public function update()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("UPDATE game_table SET guid = :guid WHERE id = :id");
		$flag = $sth->execute(array('id' => $this->getId(), 'guid' => $this->getGuid()));
		
		return $flag;
	}
	
	public function delete()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("DELETE FROM game_table WHERE id = :id");
		$flag = $sth->execute(array('id' => $this->getId()));
		
		return $flag;
	}
}

?>