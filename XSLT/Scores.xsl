<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
        version="1.0"
        xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <!-- 
      On a un XSL pour faire une sorte de classement,
      On a le meilleur score, 
     le top 5, 
     et le joueur avec plus de temps de jeu.
    -->
   
    <xsl:variable name="Gagnant">
        <xsl:for-each select="Scores/ListeScores">
            <xsl:sort select="Score" data-type="number" order="descending"/>
            <xsl:if test="position()=1">
                <xsl:value-of select="Pseudo"/>
            </xsl:if>
        </xsl:for-each>
    </xsl:variable>

    <!-- celui qui a passé une éternité à jouer... -->
    <xsl:variable name="tortue">
        <xsl:for-each select="Scores/ListeScores">
            <xsl:sort select="Temps" data-type="number" order="descending"/>
            <xsl:if test="position()=1">
                <xsl:value-of select="Pseudo"/>
            </xsl:if>
        </xsl:for-each>
    </xsl:variable>

    <xsl:template match="/">
        <html>
            <head>
                <title>classement random</title>
                <style type="text/css">
                    /* j'ai un peu fait n'imp avec le style */
                    body { background:#dfdfdf; margin:0; padding:25px;font-family:Calibri, sans-serif; }
                    .wrapper{
                    background:#ffffff;
                    padding:10px;
                    border:2px solid #777;
                    width:68%;
                    margin:0 auto;
                    margin-top:30px;
                    }
                    h1 { text-align:center; color:#333; font-size:1.5em; margin-bottom:10px;}
                    h2 { text-align:center; color:#555; margin-bottom:8px; }
                    table { width:100%; border-collapse:collapse; margin-top:20px; }
                    th, td { border:1px solid #aaa; padding:7px; font-size:13px; text-align:center; }
                    th {background:#f1f1f1;}
                    tr:nth-child(even) { background:#f9f9f9; }
                    .spe {color:#d00; font-weight:bold;}
                    .info {margin-top:20px; font-style:italic; text-align:center; color:#555; font-size:0.9em;}
                </style>
            </head>
            <body>
                <div class="wrapper">
                    <!-- franchement, on a juste mis un trophée pour le fun -->
                    <h1>🏆 Le meilleur score: <span class="spe"><xsl:value-of select="$Gagnant"/></span></h1>

                    <h2>Top 5 des gens</h2>
                    <table>
                        <tr>
                            <th>pos</th>
                            <th>pseudo</th>
                            <th>score</th>
                            <th>date</th>
                            <th>temps</th>
                        </tr>

                        <xsl:for-each select="Scores/ListeScores">
                            <xsl:sort select="Score" data-type="number" order="descending"/>
                            <xsl:if test="position() &lt;= 5">
                                <tr>
                                    <td><xsl:value-of select="position()"/></td>
                                    <td><xsl:value-of select="Pseudo"/></td>
                                    <td><xsl:value-of select="Score"/></td>
                                    <td><xsl:value-of select="Date"/></td>
                                    <td><xsl:value-of select="Temps"/></td>
                                </tr>
                            </xsl:if>
                        </xsl:for-each>

                    </table>

                    <div class="info">
                        Joueur le plus long à finir (ou pas ): <span class="spe"><xsl:value-of select="$tortue"/></span><br/>
                        (Franchement, chapeau, il a dû s'ennuyer)
                    </div>
                </div>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>
