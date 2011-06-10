<?php

require_once 'Db_Singleton.class.php'; 
require_once 'Db_Object.interface.php';
require_once 'Xml_Serializable_Object.class.php'; 

final class User extends Xml_Serializable_Object implements Db_Object
{
	protected $id;
	protected $device_unique_id;
	protected $nickname;

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
				else if(isset($args['device_unique_id']))
				{
					$this->setDeviceUniqueId($args['device_unique_id']);
				}
				else if(isset($args['nickname']))
				{
					$this->setNickname($args['nickname']);
				}
				break;
			case 2:
				if(isset($args['device_unique_id']))
				{
					$this->setDeviceUniqueId($args['device_unique_id']);
				}
				if(isset($args['nickname']))
				{
					$this->setNickname($args['nickname']);
				}
				break;
			case 3:
				if(isset($args['id']))
				{
					$this->setId($args['id']);
				}
				if(isset($args['device_unique_id']))
				{
					$this->setDeviceUniqueId($args['device_unique_id']);
				}
				if(isset($args['nickname']))
				{
					$this->setNickname($args['nickname']);
				}
				break;
		}
		$this->read();
	}
	
	public function __destruct()
	{
		$this->id = null;
		$this->device_unique_id = null;
		$this->nickname = null;
	}
	
	public function setId($id)
	{
		$this->id = abs(intval($id));
	}
	
	public function getId()
	{
		return $this->id;
	}
	
	public function setDeviceUniqueId($device_unique_id)
	{
		$this->device_unique_id = substr($device_unique_id, 0, min(20, strlen($device_unique_id)));
	}
	
	public function getDeviceUniqueId()
	{
		return $this->device_unique_id;
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
		$sth = $dbh->prepare("INSERT INTO user_table (device_unique_id, nickname) VALUES (:device_unique_id, :nickname)");
		$flag = $sth->execute(array('device_unique_id' => $this->getDeviceUniqueId(), 'nickname' => $this->getNickname()));
		
		if($flag)
		{
			$this->setId($dbh->lastInsertId());
		}
		
		return $flag;
	}
	
	public function read()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("SELECT * FROM user_table WHERE (id = :id) OR (device_unique_id = :device_unique_id) OR (nickname = :nickname) LIMIT 0, 1");
		$flag = $sth->execute(array('id' => $this->getId(), 'device_unique_id' => $this->getDeviceUniqueId(), 'nickname' => $this->getNickname()));
		
		if($flag)
		{
			$sth->setFetchMode(PDO::FETCH_ASSOC);
			$row = $sth->fetch();
			
			if($row)
			{
				$this->setId($row['id']);
				$this->setDeviceUniqueId($row['device_unique_id']);
				$this->setNickname($row['nickname']);
			}
		}
		
		return $flag;
	}
	
	public function update()
	{
		$dbh = Db_Singleton::instance();
		$sth = $dbh->prepare("UPDATE user_table SET device_unique_id = :device_unique_id, nickname = :nickname WHERE id = :id");
		$flag = $sth->execute(array('id' => $this->getId(), 'device_unique_id' => $this->getDeviceUniqueId(), 'nickname' => $this->getNickname()));
		
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