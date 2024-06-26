USE [master]
GO

/****** Object:  Database [Tekton]    Script Date: 12/06/2024 11:00:07 ******/
CREATE DATABASE [Tekton]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Tekton', FILENAME = N'C:\Users\mregalado\Tekton.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Tekton_log', FILENAME = N'C:\Users\mregalado\Tekton_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Tekton].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Tekton] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [Tekton] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [Tekton] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [Tekton] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [Tekton] SET ARITHABORT OFF 
GO

ALTER DATABASE [Tekton] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [Tekton] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [Tekton] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [Tekton] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [Tekton] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [Tekton] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [Tekton] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [Tekton] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [Tekton] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [Tekton] SET  DISABLE_BROKER 
GO

ALTER DATABASE [Tekton] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [Tekton] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [Tekton] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [Tekton] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [Tekton] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [Tekton] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [Tekton] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [Tekton] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [Tekton] SET  MULTI_USER 
GO

ALTER DATABASE [Tekton] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Tekton] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Tekton] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Tekton] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [Tekton] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Tekton] SET QUERY_STORE = OFF
GO

USE [Tekton]
GO

ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO

ALTER DATABASE [Tekton] SET  READ_WRITE 
GO


