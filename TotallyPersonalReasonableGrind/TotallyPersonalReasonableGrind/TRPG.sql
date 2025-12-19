-- MySQL dump 10.13  Distrib 8.0.44, for Win64 (x86_64)
--
-- Host: localhost    Database: totallyreasonablepersonalgrind
-- ------------------------------------------------------
-- Server version	8.0.44

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `area`
--

DROP TABLE IF EXISTS `area`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `area` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `required_lvl` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `area`
--

LOCK TABLES `area` WRITE;
/*!40000 ALTER TABLE `area` DISABLE KEYS */;
INSERT INTO `area` VALUES (1,'Plain',0),(2,'Forest',2),(3,'Beach',4),(4,'Swamp',6),(5,'Ocean',8),(6,'Desert',10),(7,'Volcano',12),(8,'Sky',14);
/*!40000 ALTER TABLE `area` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventory`
--

DROP TABLE IF EXISTS `inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventory` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `player_id` int unsigned NOT NULL,
  `item_id` int unsigned NOT NULL,
  `quantity` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `FK_INVENTORY_PLAYER_ID_idx` (`player_id`),
  KEY `FK_INVENTORY_ITEM_ID_idx` (`item_id`),
  CONSTRAINT `FK_INVENTORY_ITEM_ID` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_INVENTORY_PLAYER_ID` FOREIGN KEY (`player_id`) REFERENCES `player` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventory`
--

LOCK TABLES `inventory` WRITE;
/*!40000 ALTER TABLE `inventory` DISABLE KEYS */;
/*!40000 ALTER TABLE `inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item`
--

DROP TABLE IF EXISTS `item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `item` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `sell_value` int unsigned NOT NULL,
  `emoji_name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item`
--

LOCK TABLES `item` WRITE;
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` VALUES (1,'Mushroom',2,'mushroom'),(2,'Rock',2,'rock'),(3,'Grass',1,'leafy_green'),(4,'Slime',2,'green_circle'),(5,'Leather',3,'ox'),(6,'Wool',1,'sheep'),(7,'Wood',5,'wood'),(8,'Resin',4,'honey_pot'),(9,'Brown Mushroom',3,'brown_mushroom'),(10,'Flesh',3,'bacon'),(11,'Bone',4,'bone'),(12,'Spore',3,'bubbles'),(13,'Sea Shell',8,'shell'),(14,'Water',3,'droplet'),(15,'Crab',11,'crab'),(16,'Hermit Crab',9,'shell'),(17,'Red Slime',4,'red_circle'),(18,'Tanned Bone',7,'bone'),(19,'Kelp',15,'leafy_green'),(20,'Rotten Wood',10,'wood'),(21,'Lilypad',20,'lotus'),(22,'Pirannah',10,'tropical_fish'),(23,'Flooded Flesh',5,'bacon'),(24,'Scale',4,'feather'),(25,'Coral',10,'coral'),(26,'Plastic',4,'milk'),(27,'Fiber Optic Cable',25,'flute'),(28,'Fish',9,'fish'),(29,'Coral Scale',16,'coral'),(30,'Relic',30,'coin'),(31,'Sand',8,'desert'),(32,'Cactus',18,'cactus'),(33,'Oil',50,'oil'),(34,'Burned Flesh',9,'bacon'),(35,'Snake',16,'snake'),(36,'Web',40,'spider_web'),(37,'Volcanic Rock',10,'rock'),(38,'Obsidian',20,'rock'),(39,'Lava',65,'drop_of_blood'),(40,'Dragon Scale',75,'feather'),(41,'Magma Ball',23,'orange_circle'),(42,'Seared Bone',15,'bone'),(43,'Cloud',100,'cloud'),(44,'Star',150,'star'),(45,'Sun',500,'sun'),(46,'Wing',100,'wing'),(47,'Stelar Dust',50,'star2'),(48,'Halo',300,'innocent');
/*!40000 ALTER TABLE `item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loot`
--

DROP TABLE IF EXISTS `loot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `loot` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `item_id` int unsigned NOT NULL,
  `area_id` int unsigned NOT NULL,
  `quantity` int unsigned NOT NULL,
  `type` varchar(45) NOT NULL,
  `rarity` varchar(45) NOT NULL,
  `required_lvl` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `FK_LOOT_ITEM_ID_idx` (`item_id`),
  KEY `FK_LOOT_AREA_ID_idx` (`area_id`),
  CONSTRAINT `FK_LOOT_AREA_ID` FOREIGN KEY (`area_id`) REFERENCES `area` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_LOOT_ITEM_ID` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loot`
--

LOCK TABLES `loot` WRITE;
/*!40000 ALTER TABLE `loot` DISABLE KEYS */;
INSERT INTO `loot` VALUES (1,1,1,2,'Walk','UnCommon',1),(2,2,1,3,'Walk','Common',0),(3,3,1,5,'Walk','Common',0),(4,4,1,2,'Hit','UnCommon',1),(5,5,1,2,'Hit','Common',0),(6,6,1,3,'Hit','Common',0),(7,7,2,2,'Walk','Common',2),(8,8,2,3,'Walk','UnCommon',2),(9,9,2,2,'Walk','UnCommon',3),(10,10,2,3,'Hit','Common',2),(11,11,2,2,'Hit','Common',2),(12,12,2,4,'Hit','UnCommon',3),(13,13,3,2,'Walk','Rare',4),(14,14,3,5,'Walk','Common',4),(15,15,3,1,'Walk','Rare',5),(16,16,3,1,'Hit','UnCommon',4),(17,17,3,3,'Hit','Common',5),(18,18,3,2,'Hit','Rare',5),(19,19,4,3,'Walk','Common',6),(20,20,4,2,'Walk','UnCommon',6),(21,21,4,2,'Walk','Rare',7),(22,22,4,2,'Hit','Rare',6),(23,23,4,3,'Hit','UnCommon',7),(24,24,4,4,'Hit','Common',6),(25,25,5,3,'Walk','Rare',8),(26,26,5,6,'Walk','UnCommon',8),(27,27,5,1,'Walk','Epic',9),(28,28,5,3,'Hit','Common',8),(29,29,5,2,'Hit','Rare',8),(30,30,5,1,'Hit','Epic',9),(31,31,6,4,'Walk','Common',10),(32,32,6,2,'Walk','Rare',10),(33,33,6,1,'Walk','Epic',11),(34,34,6,4,'Hit','UnCommon',11),(35,35,6,2,'Hit','Rare',10),(36,36,6,1,'Hit','Rare',11),(37,37,7,5,'Walk','UnCommon',12),(38,38,7,3,'Walk','Rare',12),(39,39,7,1,'Walk','Epic',13),(40,40,7,1,'Hit','Legendary',13),(41,41,7,3,'Hit','Rare',12),(42,42,7,4,'Hit','UnCommon',12),(43,43,8,2,'Walk','Rare',14),(44,44,8,3,'Walk','Epic',15),(45,45,8,1,'Walk','Legendary',15),(46,46,8,2,'Hit','Rare',14),(47,47,8,10,'Hit','Epic',15),(48,48,8,1,'Hit','Legendary',15);
/*!40000 ALTER TABLE `loot` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `player`
--

DROP TABLE IF EXISTS `player`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `player` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(32) NOT NULL,
  `combat_exp` int DEFAULT NULL,
  `combat_lvl` int DEFAULT NULL,
  `exploration_exp` int DEFAULT NULL,
  `exploration_lvl` int DEFAULT NULL,
  `area_id` int unsigned DEFAULT NULL,
  `money` int unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`),
  KEY `FK_PLAYER_AREA_ID_idx` (`area_id`),
  CONSTRAINT `FK_PLAYER_AREA_ID` FOREIGN KEY (`area_id`) REFERENCES `area` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `player`
--

LOCK TABLES `player` WRITE;
/*!40000 ALTER TABLE `player` DISABLE KEYS */;
INSERT INTO `player` VALUES (1,'theodu30_',0,0,0,0,1,0),(3,'PiwoZz',0,0,0,0,1,0),(4,'TheMoonBeard',0,0,0,0,1,0);
/*!40000 ALTER TABLE `player` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-12-19 17:20:26
