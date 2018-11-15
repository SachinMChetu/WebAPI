﻿<!DOCTYPE html>
<!--[if lt IE 7 ]><html class="ie ie6" lang="en"> <![endif]-->
<!--[if IE 7 ]><html class="ie ie7" lang="en"> <![endif]-->
<!--[if IE 8 ]><html class="ie ie8" lang="en"> <![endif]-->
<!--[if (gte IE 9)|!(IE)]><!-->
<html lang="en" xmlns="http://www.w3.org/1999/html"> <!--<![endif]-->
<head>
    <!-- Basic Page Needs
    ================================================== -->
    <meta charset="utf-8" />
    <title>PointQA</title>
    <meta name="description" content="">
    <meta name="author" content="">
    <!--<meta name="viewport" content="width=device-width; initial-scale=1; maximum-scale=1">-->
    
    <!--[if lt IE 9]>
    <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
    <![endif]-->
    
    <!-- CSS ================================================== -->
    
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:100,400,300,600,700,800' rel='stylesheet' type='text/css'>
    
    <link rel="stylesheet" href="css/style.css" type="text/css">
    <link rel="stylesheet" href="css/fonts.css" type="text/css" />
    <link rel="stylesheet" href="css/font-awesome.css" type="text/css">

    <script src="http://code.jquery.com/jquery-1.10.2.min.js"></script>

    <!-- Pick-a-Date resources -->
    <link rel="stylesheet" href="css/pickadate/default.css" type="text/css" />
    <link rel="stylesheet" href="css/pickadate/default.date.css" type="text/css" />
    <link rel="stylesheet" href="css/pickadate/default.time.css" type="text/css" />
    <script src="js/pickadate/compressed/picker.js"></script>
    <script src="js/pickadate/compressed/picker.date.js"></script>

    <!-- Custom scroll resources -->
    <link rel="stylesheet" href="css/custom-scrollbar/jquery.mCustomScrollbar.css" type="text/css" />
    <script src="js/custom-scrollbar/jquery.mousewheel.min.js"></script>
    <script src="js/custom-scrollbar/jquery.mCustomScrollbar.min.js"></script>

    <!-- Flot Charts resources -->
    <script language="javascript" type="text/javascript" src="js/flot/excanvas.min.js"></script>
    <script language="javascript" type="text/javascript" src="js/flot/jquery.flot.js"></script>
    <script language="javascript" type="text/javascript" src="js/flot/jquery.flot.pie.js"></script>

    <!-- Main script -->
    <script language="javascript" type="text/javascript" src="js/main.js"></script> 

</head>


<body class="dashboard">


	<header class="header">
    
    	<a href="#" class="logo">PointQA</a>
        <nav class="header-tabs">
            <ul>
                <li><a href="default.aspx">Home</a></li>
                <li><a href="#">Calls History</a></li>
            </ul>
        </nav><!-- close header-tabs -->
        	
        
        <div class="header-actions">
        	<a href="#" class="main-cta"><i class="fa fa-headphones"></i>&nbsp;&nbsp;LISTEN</a>
            
            <div class="notifications">
            	<a href="#" class="messages"><span class="count">1</span></a>
                <a href="#" class="events"><span class="count">3</span></a>
                <a href="#" class="statistics"></a>
            </div><!-- close notifications -->
            
            
            <div class="account-section">
            	<img src="images/placeholders/Logo_no_words.png" class="my-avatar" />
                <span>
                	<strong>James Smith Jr.</strong>
                    <small>administrator</small>
                </span>
                <em><i class="fa fa-caret-down"></i></em>
            </div><!-- close account-section -->
        </div><!-- close header-actions -->
    
    <!-- close header -->
    
    
    
    <aside class="main-sidebar">
    	<div class="quick-search">
        	<form action="" method="">
            	<i class="fa fa-search"></i>
            	<input type="search" placeholder="Quick Search..." />
            </form>
        </div><!-- close quick-search -->
        
        
        <div class="navigation-and-statistics">
        
            <div class="main-navigation">
            	<ul>
                	<li class="you-are-here"><a href="#"><i class="fa fa-desktop"></i> Dashboard</a></li>
                    <li><a href="#"><i class="fa fa-user"></i> Users</a></li>
                    <li><a href="#"><i class="fa fa-cloud-upload"></i> Uploads</a></li>
                    <li><a href="#"><i class="fa fa-pencil"></i> Management</a></li>
                    <li><a href="#"><i class="fa fa-bar-chart-o"></i> Statistics</a></li>
                    <li><a href="#"><i class="fa fa-gear"></i> System</a></li>
                </ul>
                
                <a href="#" class="main-cta"><i class="fa fa-headphones"></i>&nbsp;&nbsp;LISTEN</a>
            </div><!-- close main-navigation -->
            
            <div class="daily-statistics">
            	<div class="sidebar-title">
                	<strong>TODAY</strong>
                    <a href="#">DETAILS</a>
                </div><!-- close sidebar-title -->
                
                <ul>
                	<li>
                    	<i class="fa fa-clipboard"></i>
                        <span>Records <br />Worked</span>
                        <strong>180</strong>
                    </li>
                    
                    <li>
                    	<i class="fa fa-exclamation-circle"></i>
                        <span>LEFT TO <br />REVIEW</span>
                        <strong>180</strong>
                    </li>
                    
                    <li>
                    	<i class="fa fa-bar-chart-o"></i>
                        <span>AVG DAILY <br />SCORE</span>
                        <strong>96.81</strong>
                    </li>
                    
                    <li>
                    	<i class="fa fa-headphones"></i>
                        <span>CALLS <br />REVIEWED</span>
                        <strong>2,897</strong>
                    </li>
                    
                    <li>
                    	<i class="fa fa-times-circle"></i>
                        <span>TOTAL <br />FAILS</span>
                        <strong>441</strong>
                    </li>
                </ul>
                
                
            </div><!-- close daily-statistics -->

        </div> <!-- close navigation-and-statistics -->
            
    </aside><!-- close main-sidebar -->
    
    
    
    
    <section class="main-container">
    	
        
        	<h1 class="section-title"><i class="fa fa-desktop"></i> Dashboard</h1>
            <a href="#" class="third-priority-buttom"><i class="fa fa-gear"></i> Edit Dashboard</a>
        
        
        <div class="general-filter">
        	<div class="yellow-container">
                <form action="" method="" class="">
                    <div class="field-holder search-holder">
                    	<i class="fa fa-search"></i>
                        <input type="search" placeholder="Quick Search..." />
                    </div><!-- close search-holder -->
                    
                    <div class="field-holder">
                        <i class="fa fa-mobile-phone"></i>
                        <select>
                            <option>Phone Number</option>
                        </select>
                    </div><!-- close select-holder -->
                    
                    <div class="field-holder">
                        <i class="fa fa-user"></i>
                        <select>
                            <option>Agent Group</option>
                        </select>
                    </div><!-- close select-holder -->
                    
                    <div class="field-holder">
                        <i class="fa fa-calendar-o"></i>
                        <input type="text" class="datepicker start-date" placeholder="Start..." />
                    </div><!-- close date-holder -->
                    
                    
                    <div class="field-holder">
                        <i class="fa fa-calendar-o"></i>
                        <input type="text" class="datepicker end-date" placeholder="End..." />
                    </div><!-- close date-holder -->
                    
                    
                    <button type="submit" class="secondary-cta">APPLY</button>
                </form>
            </div><!-- close yellow-container -->
            
            <div class="applied-filters">
            	<label>Aplied Filters:</label>
                
                <span>
                	<i class="fa fa-mobile-phone"></i>
                    <em>Phone: <strong>N/A</strong></em>
                </span>
                
                
                <span>
                	<i class="fa fa-user"></i>
                    <em>Group/s: <strong>All Groups</strong></em>
                </span>
                
                
                <span>
                	<i class="fa fa-clock-o"></i>
                    <em>Period: <strong>02/12/2013 - 01/04/2013</strong></em>
                </span>
                
            </div><!-- close applied-filters -->
        </div><!-- close general-filter -->
        
        
        
        
        
        <div class="panel">
        	<div class="panel-title">
            	<i class="fa fa-clock-o section-icon"></i>
                <hgroup>
                	<h1>Call Reviewed History</h1>
                    <h2>Score chart for <strong>02/12/2013 - 01/04/2013</strong></h2>
                </hgroup>
                
                <div class="panel-actions">
                	<a href="#" class="third-priority-buttom"><i class="fa fa-user"></i> VIEW TOP AGENTS</a>
                    <a href="#" class="third-priority-buttom"><i class="fa fa-download"></i> EXPORT RESULTS</a>
                </div><!-- close panel-actions -->
            </div><!-- close panel-title -->
            
            <div class="panel-content">

                
                <div class="feed-agents-list">
                	<div class="tooltip-title">
                    	<div class="agents-count">
                        	<span>AGENTS WITH</span>
                            <strong>80 ~ 90 <small>points</small></strong>
                        </div><!-- close agents-count -->
                        
                        <div class="tooltip-title-actions">
                        	<span>182 Agents</span>
                            <a href="#">VIEW FULL LIST</a>
                        </div><!-- close tooltip-title-actions -->
                    </div><!-- close tooltip-title -->
                    
                    
                    <div class="scrolling-list">
                        <ul>
                        	<li>
                            	<a href="#">
                                	<i class="fa fa-chevron-right"></i>
                                	<img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                    	<h1>John Smith</h1>
                                        <span><strong>81.97</strong> Avg Score</span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                            	<a href="#">
                                	<i class="fa fa-chevron-right"></i>
                                	<img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                    	<h1>John Smith</h1>
                                        <span><strong>81.97</strong> Avg Score / <em>2 fails today</em></span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                            	<a href="#">
                                	<i class="fa fa-chevron-right"></i>
                                	<img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                    	<h1>John Smith</h1>
                                        <span><strong>81.97</strong> Avg Score</span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                            	<a href="#">
                                	<i class="fa fa-chevron-right"></i>
                                	<img src="images/female-avatar.png" class="male-avatar" />
                                    <div>
                                    	<h1>John Smith</h1>
                                        <span><strong>81.97</strong> Agent Avg Score / <em>2 fails today</em></span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                            	<a href="#">
                                	<i class="fa fa-chevron-right"></i>
                                	<img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                    	<h1>John Smith</h1>
                                        <span><strong>81.97</strong> Avg Score</span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                            	<a href="#">
                                	<i class="fa fa-chevron-right"></i>
                                	<img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                    	<h1>John Smith</h1>
                                        <span><strong>81.97</strong> Agent Avg Score / <em>2 fails today</em></span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                            	<a href="#">
                                	<i class="fa fa-chevron-right"></i>
                                	<img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                    	<h1>John Smith</h1>
                                        <span><strong>81.97</strong> Avg Score</span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                            	<a href="#">
                                	<i class="fa fa-chevron-right"></i>
                                	<img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                    	<h1>John Smith</h1>
                                        <span><strong>81.97</strong> Agent Avg Score / <em>2 fails today</em></span>
                                    </div>
                                </a>
                            </li>

                        <li>
                                <a href="#">
                                    <i class="fa fa-chevron-right"></i>
                                    <img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                        <h1>John Smith</h1>
                                        <span><strong>81.97</strong> Avg Score</span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                                <a href="#">
                                    <i class="fa fa-chevron-right"></i>
                                    <img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                        <h1>John Smith</h1>
                                        <span><strong>81.97</strong> Avg Score / <em>2 fails today</em></span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                                <a href="#">
                                    <i class="fa fa-chevron-right"></i>
                                    <img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                        <h1>John Smith</h1>
                                        <span><strong>81.97</strong> Avg Score</span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                                <a href="#">
                                    <i class="fa fa-chevron-right"></i>
                                    <img src="images/female-avatar.png" class="male-avatar" />
                                    <div>
                                        <h1>John Smith</h1>
                                        <span><strong>81.97</strong> Agent Avg Score / <em>2 fails today</em></span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                                <a href="#">
                                    <i class="fa fa-chevron-right"></i>
                                    <img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                        <h1>John Smith</h1>
                                        <span><strong>81.97</strong> Avg Score</span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                                <a href="#">
                                    <i class="fa fa-chevron-right"></i>
                                    <img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                        <h1>John Smith</h1>
                                        <span><strong>81.97</strong> Agent Avg Score / <em>2 fails today</em></span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                                <a href="#">
                                    <i class="fa fa-chevron-right"></i>
                                    <img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                        <h1>John Smith</h1>
                                        <span><strong>81.97</strong> Avg Score</span>
                                    </div>
                                </a>
                            </li>
                            
                            
                            <li>
                                <a href="#">
                                    <i class="fa fa-chevron-right"></i>
                                    <img src="images/male-avatar.png" class="male-avatar" />
                                    <div>
                                        <h1>John Smith</h1>
                                        <span><strong>81.97</strong> Agent Avg Score / <em>2 fails today</em></span>
                                    </div>
                                </a>
                            </li>                            
                        </ul>
                    </div><!-- close scrolling-list -->
                </div><!-- close feed-agents-list -->

                <div class="chart-results">
                
                    <div class="chart-container">
                        <div class="chart">
                            <div id="chart-placeholder"></div>
                            <span>
                                <strong>72.22</strong>
                                <small>AVG SCORE</small>
                            </span>
                        </div><!-- close chart -->
                        
                        <div class="change-chart">
                            <a href="#" class="selected-chart"><i class="fa fa-bar-chart-o"></i></a>
                            <a href="#"><i class="fa fa-bar-chart-o"></i></a>
                            <a href="#"><i class="fa fa-bar-chart-o"></i></a>
                        </div><!-- clos change-chart -->
                    </div><!-- close chart-container -->
                    
                    
                    <div class="results">
                        <h1>Results</h1>
                        
                        <ul>
                            <li>
                                <div class="color red-color"></div>
                                <strong>0 points</strong>
                                <span>(128 Agents)</span>
                            </li>
                            
                            
                            <li>
                                <div class="color green-color"></div>
                                <strong>70 ~ 80 points</strong>
                                <span>(318 Agents)</span>
                            </li>
                            
                            
                            <li>
                                <div class="color yellow-color"></div>
                                <strong>80 ~ 90 points</strong>
                                <span>(138 Agents)</span>
                            </li>
                            
                            
                            
                            <li>
                                <div class="color orange-color"></div>
                                <strong>100 points</strong>
                                <span>(118 Agents)</span>
                            </li>
                            
                            
                            
                            <li>
                                <div class="color dark-orange-color"></div>
                                <strong>Other</strong>
                                <span>(148 Agents)</span>
                            </li>
                        </ul>
                        
                        <div class="total-count">
                            <span>AVG SCORE</span>
                            <strong>71.22</strong>
                            
                            <a href="#" class="third-priority-buttom right-oriented-icon">COMPARE <i class="fa fa-chevron-right"></i></a>
                        </div><!-- close total-count -->
                        
                    </div><!-- close results -->
                    
                </div><!-- close chart-results -->
                                
            </div><!-- close panel-content -->
        </div><!-- close panel -->
        
        
        
        <div class="panel">
        	<div class="panel-title">
            	<i class="fa fa-mobile-phone section-icon"></i>
                <hgroup>
                	<h1>Calls Details</h1>
                    <h2>Calls Details for <strong>02/12/2013 - 01/04/2013</strong></h2>
                </hgroup>
            </div><!-- close panel-title -->
            
            <div class="panel-content">
            	

                <div class="calls-additional-details">
                
                	<div class="secondary-panel">
                    	<div class="panel-title">
                        	<h1>Top Missed Points</h1>
                            <a href="#">VIEW ALL</a>
                        </div><!-- close panel-title -->
                        
                        <div class="panel-contant">
                        	<table class="invisible-frame">
                            	<tbody>
                                	<tr>
                                    	<td><div class="color red-color"></div></td>
                                        <td><a href="#">TCPA Close</a></td>
                                        <td class="text-align-right"><strong>92%</strong></td>
                                        <td class="text-align-right"><small>(943 Agents)</small></td>
                                    </tr>
                                    
                                    
                                    <tr>
                                    	<td><div class="color dark-orange-color"></div></td>
                                        <td><a href="#">TCPA Close</a></td>
                                        <td class="text-align-right"><strong>92%</strong></td>
                                        <td class="text-align-right"><small>(943 Agents)</small></td>
                                    </tr>
                                    
                                    
                                    <tr>
                                    	<td><div class="color orange-color"></div></td>
                                        <td><a href="#">TCPA Close</a></td>
                                        <td class="text-align-right"><strong>92%</strong></td>
                                        <td class="text-align-right"><small>(943 Agents)</small></td>
                                    </tr>
                                    
                                    
                                    <tr>
                                    	<td><div class="color yellow-color"></div></td>
                                        <td><a href="#">TCPA Close</a></td>
                                        <td class="text-align-right"><strong>92%</strong></td>
                                        <td class="text-align-right"><small>(943 Agents)</small></td>
                                    </tr>
                                    
                                    
                                    <tr>
                                    	<td><div class="color green-color"></div></td>
                                        <td><a href="#">TCPA Close</a></td>
                                        <td class="text-align-right"><strong>92%</strong></td>
                                        <td class="text-align-right"><small>(943 Agents)</small></td>
                                    </tr>
                                </tbody>
                            </table><!-- close invisible-frame -->
                        </div><!-- close panel-content -->
                    </div><!-- close secondary-panel -->
                    
                    
                    
                    
                    
                    <div class="secondary-panel">
                    	<div class="panel-title">
                        	<h1>Repeated Points</h1>
                            <a href="#">VIEW ALL</a>
                        </div><!-- close panel-title -->
                        
                        <div class="panel-contant">
                        	<table class="invisible-frame">
                            	<tbody>
                                	<tr>
                                    	<td><div class="color red-color"></div></td>
                                        <td><a href="#">TCPA Close</a></td>
                                        <td class="text-align-right">12</td>
                                        <td class="text-align-center"><a href="#" title="John Smith, ID: 122823664"><img src="images/male-avatar.png" /></a></td>
                                    </tr>
                                    
                                    
                                    <tr>
                                    	<td><div class="color dark-orange-color"></div></td>
                                        <td><a href="#">TCPA Close</a></td>
                                        <td class="text-align-right">12</td>
                                        <td class="text-align-center"><a href="#" title="John Smith, ID: 122823664"><img src="images/male-avatar.png" /></a></td>
                                    </tr>
                                    
                                    
                                    <tr>
                                    	<td><div class="color orange-color"></div></td>
                                        <td><a href="#">TCPA Close</a></td>
                                        <td class="text-align-right">12</td>
                                        <td class="text-align-center"><a href="#" title="John Smith, ID: 122823664"><img src="images/male-avatar.png" /></a></td>
                                    </tr>
                                    
                                    
                                    <tr>
                                    	<td><div class="color yellow-color"></div></td>
                                        <td><a href="#">TCPA Close</a></td>
                                        <td class="text-align-right">12</td>
                                        <td class="text-align-center"><a href="#" title="John Smith, ID: 122823664"><img src="images/male-avatar.png" /></a></td>
                                    </tr>
                                    
                                    
                                    <tr>
                                    	<td><div class="color green-color"></div></td>
                                        <td><a href="#">TCPA Close</a></td>
                                        <td class="text-align-right">12</td>
                                        <td class="text-align-center"><a href="#" title="John Smith, ID: 122823664"><img src="images/male-avatar.png" /></a></td>
                                    </tr>
                                </tbody>
                            </table><!-- close invisible-frame -->
                        </div><!-- close panel-content -->
                    </div><!-- close secondary-panel -->
                    
                    
                    
                    
                    
                </div><!-- close calls-additional-details -->
                

                <div class="calls-list">
                    <div class="sub-title">
                        <h1>QA Details</h1>
                        <div class="sub-title-actions">
                            <a href="#" class="third-priority-buttom"><i class="fa fa-print"></i> PRINT REPORT</a>
                            <a href="#" class="third-priority-buttom"><i class="fa fa-download"></i> EXPORT REPORT</a>
                        </div><!-- close sub-title-actions -->
                    </div><!-- close sub-title -->
                    
                    
                    <div class="table-outline">
                        <table>
                            <thead>
                                <tr>
                                    <td><span>Agent</span> <a href="#"><i class="fa fa-caret-down"></i></a></td>
                                    <td>Phone</td>
                                    <td><span>Call Time</span> <a href="#"><i class="fa fa-caret-down"></i></a></td>
                                    <td><span>Score</span> <a href="#"><i class="fa fa-caret-down"></i></a></td>
                                    <td><span>Missed Points</span> <a href="#"><i class="fa fa-caret-down"></i></a></td>
                                    <td><span>Results</span> <a href="#"><i class="fa fa-caret-down"></i></a></td>
                                </tr>
                            </thead>
                            
                            
                            <tbody>
                                <tr>
                                    <td class="first-cell text-align-left">
                                        <div class="first-cell-container">
                                            <span class="result-indicator final-success"></span>
                                            <img src="images/male-avatar.png" />
                                            <span>
                                                <strong>Maxim Chernikov</strong>
                                                <small>ID: 123123123123</small>
                                            </span>
                                        </div><!-- close first-cell-container -->
                                    </td>
                                    <td class="text-align-left"><strong>0987 654 321</strong></td>
                                    <td class="text-align-right">0:52</td>
                                    <td class="text-align-right"><em>90</em></td>
                                    <td class="text-align-center"><a href="#">3 POINTS</a></td>
                                    <td class="text-align-center"><span class="final-result">PASS <i class="fa fa-check"></i></span></td>
                                </tr>
                                
                                
                                <tr class="fail-row">
                                    <td class="first-cell">
                                        <div class="first-cell-container">
                                            <span class="result-indicator final-fail"></span>
                                            <img src="images/male-avatar.png" />
                                            <span>
                                                <strong>Maxim Chernikov</strong>
                                                <small>ID: 123123123123</small>
                                            </span>
                                        </div><!-- close first-cell-container -->
                                    </td>
                                    <td class="text-align-left"><strong>0987 654 321</strong></td>
                                    <td class="text-align-right">0:52</td>
                                    <td class="text-align-right"><em>90</em></td>
                                    <td class="text-align-center"><a href="#">3 POINTS</a></td>
                                    <td class="text-align-center"><span class="final-result">FAIL <i class="fa fa-times"></i></span></td>
                                </tr>
                                
                                
                                <tr>
                                    <td class="first-cell text-align-left">
                                        <div class="first-cell-container">
                                            <span class="result-indicator final-success"></span>
                                            <img src="images/male-avatar.png" />
                                            <span>
                                                <strong>Maxim Chernikov</strong>
                                                <small>ID: 123123123123</small>
                                            </span>
                                        </div><!-- close first-cell-container -->
                                    </td>
                                    <td class="text-align-left"><strong>0987 654 321</strong></td>
                                    <td class="text-align-right">0:52</td>
                                    <td class="text-align-right"><em>90</em></td>
                                    <td class="text-align-center"><a href="#">3 POINTS</a></td>
                                    <td class="text-align-center"><span class="final-result">PASS <i class="fa fa-check"></i></span></td>
                                </tr>
                                
                                
                                <tr>
                                    <td class="first-cell text-align-left">
                                        <div class="first-cell-container">
                                            <span class="result-indicator final-success"></span>
                                            <img src="images/female-avatar.png" />
                                            <span>
                                                <strong>Maxim Chernikov</strong>
                                                <small>ID: 123123123123</small>
                                            </span>
                                        </div><!-- close first-cell-container -->
                                    </td>
                                    <td class="text-align-left"><strong>0987 654 321</strong></td>
                                    <td class="text-align-right">0:52</td>
                                    <td class="text-align-right"><em>90</em></td>
                                    <td class="text-align-center"><a href="#">3 POINTS</a></td>
                                    <td class="text-align-center"><span class="final-result">PASS <i class="fa fa-check"></i></span></td>
                                </tr>
                                
                                
                                <tr class="fail-row">
                                    <td class="first-cell">
                                        <div class="first-cell-container">
                                            <span class="result-indicator final-fail"></span>
                                            <img src="images/male-avatar.png" />
                                            <span>
                                                <strong>Maxim Chernikov</strong>
                                                <small>ID: 123123123123</small>
                                            </span>
                                        </div><!-- close first-cell-container -->
                                    </td>
                                    <td class="text-align-left"><strong>0987 654 321</strong></td>
                                    <td class="text-align-right">0:52</td>
                                    <td class="text-align-right"><em>90</em></td>
                                    <td class="text-align-center"><a href="#">3 POINTS</a></td>
                                    <td class="text-align-center"><span class="final-result">FAIL <i class="fa fa-times"></i></span></td>
                                </tr>
                                
                                
                                <tr>
                                    <td class="first-cell text-align-left">
                                        <div class="first-cell-container">
                                            <span class="result-indicator final-success"></span>
                                            <img src="images/female-avatar.png" />
                                            <span>
                                                <strong>Maxim Chernikov</strong>
                                                <small>ID: 123123123123</small>
                                            </span>
                                        </div><!-- close first-cell-container -->
                                    </td>
                                    <td class="text-align-left"><strong>0987 654 321</strong></td>
                                    <td class="text-align-right">0:52</td>
                                    <td class="text-align-right"><em>90</em></td>
                                    <td class="text-align-center"><a href="#">3 POINTS</a></td>
                                    <td class="text-align-center"><span class="final-result">PASS <i class="fa fa-check"></i></span></td>
                                </tr>
                                
                                
                                <tr>
                                    <td class="first-cell text-align-left">
                                        <div class="first-cell-container">
                                            <span class="result-indicator final-success"></span>
                                            <img src="images/male-avatar.png" />
                                            <span>
                                                <strong>Maxim Chernikov</strong>
                                                <small>ID: 123123123123</small>
                                            </span>
                                        </div><!-- close first-cell-container -->
                                    </td>
                                    <td class="text-align-left"><strong>0987 654 321</strong></td>
                                    <td class="text-align-right">0:52</td>
                                    <td class="text-align-right"><em>90</em></td>
                                    <td class="text-align-center"><a href="#">3 POINTS</a></td>
                                    <td class="text-align-center"><span class="final-result">PASS <i class="fa fa-check"></i></span></td>
                                </tr>
                            </tbody>
                            
                            <tfoot>
                                <tr>
                                    <td colspan="6">
                                        <ul class="table-navigation">
                                            <li><a href="#">1</a></li>
                                            <li><a href="#">2</a></li>
                                            <li class="selected-page"><a href="#">3</a></li>
                                            <li><a href="#">4</a></li>
                                            <li><a href="#">5</a></li>
                                            <li><a href="#">6</a></li>
                                            <li><a href="#">7</a></li>
                                            <li><a href="#">8</a></li>
                                        </ul><!-- close table-navigation -->
                                        
                                        
                                        <div class="sort-options">
                                            <form action="" method="">
                                                <label>Show</label>
                                                <select>
                                                    <option>10</option>
                                                </select>
                                                <label>Records per page</label>
                                                <button type="button" class="secondary-cta">SET</button>
                                            </form>
                                        </div><!-- close sort-options -->
                                    </td>
                                </tr>
                            </tfoot>
                            
                        </table>    
                    </div><!-- close table-outline -->
                </div><!-- close calls-list -->                
                    

            </div><!-- close panel-content -->
            
        </div><!-- close panel -->
        
        
        
        
        
        
        
        
    </section><!-- close main-container -->
    
    

    
    
</body>
</html>
