CREATE DATABASE  IF NOT EXISTS `campingapp` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `campingapp`;
-- MySQL dump 10.13  Distrib 8.0.44, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: campingapp
-- ------------------------------------------------------
-- Server version	8.4.7

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
-- Table structure for table `accommodatie_types`
--

DROP TABLE IF EXISTS `accommodatie_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accommodatie_types` (
  `id` int NOT NULL AUTO_INCREMENT,
  `naam` varchar(50) NOT NULL,
  `prijs` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accommodatie_types`
--

LOCK TABLES `accommodatie_types` WRITE;
/*!40000 ALTER TABLE `accommodatie_types` DISABLE KEYS */;
INSERT INTO `accommodatie_types` VALUES (1,'Tent',2.99),(2,'Caravan',6.99),(3,'Camper',7.99),(4,'Chalet',49.99);
/*!40000 ALTER TABLE `accommodatie_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `faciliteiten`
--

DROP TABLE IF EXISTS `faciliteiten`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `faciliteiten` (
  `id` int NOT NULL AUTO_INCREMENT,
  `naam` varchar(100) NOT NULL,
  `omschrijving` text,
  `image_name` varchar(100) NOT NULL,
  `x_position` float NOT NULL DEFAULT '0',
  `y_position` float NOT NULL DEFAULT '0',
  `width` float NOT NULL DEFAULT '0.05',
  `height` float NOT NULL DEFAULT '0.07',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `faciliteiten`
--

LOCK TABLES `faciliteiten` WRITE;
/*!40000 ALTER TABLE `faciliteiten` DISABLE KEYS */;
INSERT INTO `faciliteiten` VALUES (1,'Wasserette','Draai hier uw wasjes. Muntjes verkrijgbaar bij de receptie!','wasserette.png',0.3521,0.305,0.02656,0.04444),(2,'Toiletten West','Toiletten in sanitairgebouw west, dagelijks gereinigd!','toilet.png',0.3521,0.355,0.02656,0.04444),(3,'Douches','Warme douches. Muntjes verkrijgbaar bij de receptie!','douches.png',0.3521,0.405,0.02656,0.04444),(4,'Zwembad','Verwarmd buitenzwembad. Geopend 09:00 - 20:00.','zwembad.png',0.421,0.2645,0.02656,0.04444),(5,'Restaurant','De Gulle Bever - Snacks en plateservice.','restaurant.png',0.5272,0.3499,0.02656,0.04444),(6,'Speeltuin','Klimrek en schommels voor de kleintjes!','speeltuin.png',0.5085,0.2315,0.02656,0.04444),(7,'Zwemmeer & Strand','Heerlijk recreatiemeer met zandstrand. Het is ook mogelijk om te barbequeen op het strand!','strand.png',0.6615,0.22,0.02656,0.04444),(8,'Parkeerplaats','Centrale parkeerplaats. Gelieve auto\'s alleen gebruiken om caravans op hun plek te zetten!','parkeerplek.png',0.651,0.466,0.02656,0.04444),(9,'Receptie','Meld u hier aan. Wij helpen u graag!','receptie.png',0.452,0.466,0.02656,0.04444),(10,'Afwasruimte','Publieke afwasruimte waar u al uw borden, bestek en andere vaat kan wassen!','afwasruimte.png',0.587,0.3527,0.02656,0.04444),(11,'Afvalbak Groepsveld','Houd de camping schoon!','afvalbak.png',0.069,0.35,0.02656,0.04444),(12,'Afvalbak Noord','Houd de camping schoon!','afvalbak.png',0.497,0.105,0.02656,0.04444),(13,'Afvalbak Zuid','Houd de camping schoon!','afvalbak.png',0.061,0.875,0.02656,0.04444),(14,'Afvalbak Staatseveld','Houd de camping schoon!','afvalbak.png',0.5665,0.515,0.02656,0.04444),(15,'Verzamelpunt','Verzamelplaats voor gezamenlijke activiteiten en in het geval van calamiteiten.','verzamelpunt.png',0.61,0.8,0.02656,0.04444),(16,'Toiletten Oost','Toiletten in sanitairgebouw oost, dagelijks gereinigd!','toilet.png',0.587,0.4027,0.02656,0.04444),(17,'Afvalbak Centraal','Houd de camping schoon.','afvalbak.png',0.3519,0.512,0.02656,0.04444);
/*!40000 ALTER TABLE `faciliteiten` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gasten`
--

DROP TABLE IF EXISTS `gasten`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `gasten` (
  `id` int NOT NULL AUTO_INCREMENT,
  `naam` varchar(100) NOT NULL,
  `geboortedatum` date NOT NULL,
  `email` varchar(150) DEFAULT NULL,
  `telefoon` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gasten`
--

LOCK TABLES `gasten` WRITE;
/*!40000 ALTER TABLE `gasten` DISABLE KEYS */;
INSERT INTO `gasten` VALUES (1,'Mark','2025-12-06','IkbenMark@outlook.com','065587322'),(2,'Sophie','1998-03-12','sophie@mail.com','0612345678'),(3,'Liam','1985-07-22','liam@test.com','0623456789'),(4,'Noah','2001-11-04','noah@example.com','0634567890'),(5,'Emma','1993-09-30','emma@mail.net','0645678901'),(6,'Lucas','1980-01-15','lucas@domain.com','0656789012'),(7,'Julia','2005-05-08','julia@email.org','0667890123'),(8,'Finn','1997-12-19','finn@webmail.com','0678901234'),(9,'Mila','1989-02-27','mila@outlook.com','0689012345'),(10,'Daan','1995-08-14','daan@provider.nl','0690123456'),(11,'Kaj Treep','1997-10-06','kajtreep@gmail.com','0646291419');
/*!40000 ALTER TABLE `gasten` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reservering_gasten`
--

DROP TABLE IF EXISTS `reservering_gasten`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reservering_gasten` (
  `reservering_id` int NOT NULL,
  `gast_id` int NOT NULL,
  PRIMARY KEY (`reservering_id`,`gast_id`),
  KEY `gast_id` (`gast_id`),
  CONSTRAINT `reservering_gasten_ibfk_1` FOREIGN KEY (`reservering_id`) REFERENCES `reserveringen` (`id`),
  CONSTRAINT `reservering_gasten_ibfk_2` FOREIGN KEY (`gast_id`) REFERENCES `gasten` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reservering_gasten`
--

LOCK TABLES `reservering_gasten` WRITE;
/*!40000 ALTER TABLE `reservering_gasten` DISABLE KEYS */;
/*!40000 ALTER TABLE `reservering_gasten` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reservering_voorzieningen`
--

DROP TABLE IF EXISTS `reservering_voorzieningen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reservering_voorzieningen` (
  `id` int NOT NULL AUTO_INCREMENT,
  `reservering_id` int NOT NULL,
  `voorziening_id` int NOT NULL,
  `aantal` int NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `reservering_id` (`reservering_id`),
  KEY `voorziening_id` (`voorziening_id`),
  CONSTRAINT `reservering_voorzieningen_ibfk_1` FOREIGN KEY (`reservering_id`) REFERENCES `reserveringen` (`id`),
  CONSTRAINT `reservering_voorzieningen_ibfk_2` FOREIGN KEY (`voorziening_id`) REFERENCES `voorzieningen` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reservering_voorzieningen`
--

LOCK TABLES `reservering_voorzieningen` WRITE;
/*!40000 ALTER TABLE `reservering_voorzieningen` DISABLE KEYS */;
/*!40000 ALTER TABLE `reservering_voorzieningen` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reserveringen`
--

DROP TABLE IF EXISTS `reserveringen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reserveringen` (
  `id` int NOT NULL AUTO_INCREMENT,
  `staanplaats_id` int NOT NULL,
  `reserveringhouder_id` int NOT NULL,
  `aankomstdatum` date NOT NULL,
  `vertrekdatum` date NOT NULL,
  `is_betaald` tinyint(1) DEFAULT '0',
  `totaal_prijs` decimal(10,2) NOT NULL,
  `accommodatie_type_id` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `staanplaats_id` (`staanplaats_id`),
  KEY `reserveringhouder_id` (`reserveringhouder_id`),
  KEY `fk_reservering_accommodatie` (`accommodatie_type_id`),
  CONSTRAINT `fk_reservering_accommodatie` FOREIGN KEY (`accommodatie_type_id`) REFERENCES `accommodatie_types` (`id`),
  CONSTRAINT `reserveringen_ibfk_1` FOREIGN KEY (`staanplaats_id`) REFERENCES `staanplaatsen` (`id`),
  CONSTRAINT `reserveringen_ibfk_2` FOREIGN KEY (`reserveringhouder_id`) REFERENCES `gasten` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reserveringen`
--

LOCK TABLES `reserveringen` WRITE;
/*!40000 ALTER TABLE `reserveringen` DISABLE KEYS */;
INSERT INTO `reserveringen` VALUES (1,6,1,'2025-12-08','2025-12-09',0,0.00,1),(2,12,1,'2025-12-08','2025-12-09',0,0.00,1),(3,4,1,'2025-12-08','2025-12-09',0,0.00,1),(4,34,1,'2025-12-08','2025-12-09',0,0.00,2),(5,36,1,'2025-12-08','2025-12-09',0,0.00,2),(6,39,1,'2025-12-08','2025-12-15',0,0.00,4),(7,18,1,'2025-12-08','2025-12-15',0,0.00,1),(8,40,1,'2025-12-08','2025-12-15',0,0.00,4),(9,37,1,'2025-12-08','2025-12-15',0,0.00,3),(10,37,1,'2025-12-23','2025-12-30',0,0.00,3),(11,42,1,'2025-12-08','2025-12-29',0,0.00,2),(12,30,1,'2025-12-08','2025-12-29',0,0.00,2),(13,1,1,'2025-12-08','2025-12-29',0,0.00,1),(14,9,1,'2025-12-08','2025-12-22',0,0.00,1),(15,21,1,'2025-12-08','2025-12-09',0,0.00,2),(16,21,1,'2025-12-10','2025-12-13',0,0.00,3),(17,39,1,'2025-12-22','2025-12-29',0,0.00,4),(18,36,1,'2025-12-25','2025-12-31',0,0.00,2),(19,11,1,'2025-12-25','2025-12-31',0,0.00,1),(20,29,1,'2025-12-15','2025-12-22',0,0.00,1),(21,8,1,'2025-12-15','2025-12-22',0,0.00,1),(22,18,1,'2025-12-15','2025-12-22',0,0.00,1),(23,31,1,'2025-12-15','2025-12-31',0,0.00,2),(24,37,11,'2025-12-16','2025-12-18',0,0.00,3);
/*!40000 ALTER TABLE `reserveringen` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `staanplaats_accommodatietypes`
--

DROP TABLE IF EXISTS `staanplaats_accommodatietypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `staanplaats_accommodatietypes` (
  `staanplaats_id` int NOT NULL,
  `accommodatie_type_id` int NOT NULL,
  PRIMARY KEY (`staanplaats_id`,`accommodatie_type_id`),
  KEY `fk_geschiktheid_type` (`accommodatie_type_id`),
  CONSTRAINT `fk_geschiktheid_staanplaats` FOREIGN KEY (`staanplaats_id`) REFERENCES `staanplaatsen` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_geschiktheid_type` FOREIGN KEY (`accommodatie_type_id`) REFERENCES `accommodatie_types` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `staanplaats_accommodatietypes`
--

LOCK TABLES `staanplaats_accommodatietypes` WRITE;
/*!40000 ALTER TABLE `staanplaats_accommodatietypes` DISABLE KEYS */;
INSERT INTO `staanplaats_accommodatietypes` VALUES (1,1),(2,1),(3,1),(4,1),(5,1),(6,1),(7,1),(8,1),(9,1),(10,1),(11,1),(12,1),(14,1),(15,1),(16,1),(18,1),(19,1),(20,1),(22,1),(23,1),(24,1),(25,1),(27,1),(29,1),(13,2),(17,2),(21,2),(26,2),(28,2),(30,2),(31,2),(32,2),(33,2),(34,2),(35,2),(36,2),(37,2),(38,2),(42,2),(13,3),(17,3),(21,3),(26,3),(28,3),(30,3),(31,3),(32,3),(33,3),(34,3),(35,3),(36,3),(37,3),(38,3),(42,3),(39,4),(40,4),(41,4);
/*!40000 ALTER TABLE `staanplaats_accommodatietypes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `staanplaatsen`
--

DROP TABLE IF EXISTS `staanplaatsen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `staanplaatsen` (
  `id` int NOT NULL AUTO_INCREMENT,
  `veld_id` int NOT NULL,
  `naam` varchar(100) NOT NULL,
  `prijs` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `veld_id` (`veld_id`),
  CONSTRAINT `staanplaatsen_ibfk_1` FOREIGN KEY (`veld_id`) REFERENCES `velden` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `staanplaatsen`
--

LOCK TABLES `staanplaatsen` WRITE;
/*!40000 ALTER TABLE `staanplaatsen` DISABLE KEYS */;
INSERT INTO `staanplaatsen` VALUES (1,2,'Staanplaats 1',20.00),(2,2,'Staanplaats 2',20.00),(3,2,'Staanplaats 3',20.00),(4,2,'Staanplaats 4',20.00),(5,2,'Staanplaats 5',20.00),(6,2,'Staanplaats 6',20.00),(7,2,'Staanplaats 7',20.00),(8,2,'Staanplaats 8',20.00),(9,2,'Staanplaats 9',20.00),(10,2,'Staanplaats 10',20.00),(11,2,'Staanplaats 11',20.00),(12,2,'Staanplaats 12',20.00),(13,3,'Staanplaats 13',35.00),(14,3,'Staanplaats 14',25.00),(15,3,'Staanplaats 15',25.00),(16,3,'Staanplaats 16',25.00),(17,3,'Staanplaats 17',35.00),(18,3,'Staanplaats 18',25.00),(19,3,'Staanplaats 19',25.00),(20,3,'Staanplaats 20',25.00),(21,3,'Staanplaats 21',35.00),(22,3,'Staanplaats 22',25.00),(23,3,'Staanplaats 23',25.00),(24,3,'Staanplaats 24',25.00),(25,4,'Staanplaats 25',25.00),(26,4,'Staanplaats 26',30.00),(27,4,'Staanplaats 27',25.00),(28,4,'Staanplaats 28',30.00),(29,4,'Staanplaats 29',25.00),(30,4,'Staanplaats 30',30.00),(31,4,'Staanplaats 31',30.00),(32,4,'Staanplaats 32',30.00),(33,4,'Staanplaats 33',30.00),(34,5,'Staanplaats 34',35.00),(35,5,'Staanplaats 35',35.00),(36,5,'Staanplaats 36',35.00),(37,5,'Staanplaats 37',35.00),(38,5,'Staanplaats 38',35.00),(39,5,'Staanplaats 39',80.00),(40,5,'Staanplaats 40',80.00),(41,5,'Staanplaats 41',80.00),(42,5,'Staanplaats 42',35.00);
/*!40000 ALTER TABLE `staanplaatsen` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `velden`
--

DROP TABLE IF EXISTS `velden`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `velden` (
  `id` int NOT NULL AUTO_INCREMENT,
  `naam` varchar(100) NOT NULL,
  `x_position` float NOT NULL DEFAULT '0',
  `y_position` float NOT NULL DEFAULT '0',
  `width` float NOT NULL DEFAULT '0',
  `height` float NOT NULL DEFAULT '0',
  `description` text,
  `image_name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `velden`
--

LOCK TABLES `velden` WRITE;
/*!40000 ALTER TABLE `velden` DISABLE KEYS */;
INSERT INTO `velden` VALUES (1,'Groepsveld',0.088,0.13,0.232,0.213,'Een groot en gezellig veld, perfect voor groepen en families die dicht bij elkaar willen staan. Dicht bij de sanitaire voorzieningen.','groepsveld.png'),(2,'Trekkersveld',0.11,0.4457,0.195,0.181,'Speciaal ingericht voor wandelaars en fietsers met kleine tentjes. Auto\'s zijn hier niet toegestaan.','trekkersveld.png'),(3,'Winterveld',0.11,0.675,0.195,0.181,'Mooi veld dat geschikt is voor winterkamperen.','winterveld.png'),(4,'Staatseveld',0.5577,0.576,0.195,0.1952,'Een rustig gelegen veld aan de rand van het bos. Ideaal voor rustzoekers.','staatseveld.png'),(5,'Oranjeveld',0.37,0.59,0.145,0.254,'Het zonnigste veld van de camping, centraal gelegen nabij de speeltuin.','oranjeveld.png');
/*!40000 ALTER TABLE `velden` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `voorzieningen`
--

DROP TABLE IF EXISTS `voorzieningen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `voorzieningen` (
  `id` int NOT NULL AUTO_INCREMENT,
  `naam` varchar(50) NOT NULL,
  `prijs` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `voorzieningen`
--

LOCK TABLES `voorzieningen` WRITE;
/*!40000 ALTER TABLE `voorzieningen` DISABLE KEYS */;
/*!40000 ALTER TABLE `voorzieningen` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-12-17 16:19:36
