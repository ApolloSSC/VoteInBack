Feature: JugementMajoritaire
	En tant que client de l'API à la clôture d'un scrutin 
	Je souhaite calculer le résultat du scrutin
	Pour obtenir la liste des résultats

Background: 
Given Une liste de scrutin
| Scrutin   |
| Scrutin 1 |
| Scrutin 2 |
And les choix suivants
	| Nom        | Valeur |
	| A rejeter  | 0      |
	| Passable   | 1      |
	| Correct    | 2      |
	| Assez bien | 3      |
	| Bien       | 4      |
	| Très bien  | 5      |
	| Excellent  | 6      |

@mytag
Scenario: JugementMajoritaire Identifier candidat vainqueur
	Given les options suivantes pour le scrutin "Scrutin 1"
	| Nom        |
	| candidat 1 |
	| candidat 2 |
	And les votes des electeurs pour le "Scrutin 1"
	| Option     | Choix      | Electeur   |
	| candidat 1 | Très bien  | electeur 1 |
	| candidat 1 | Bien       | electeur 2 |
	| candidat 1 | Bien       | electeur 3 |
	| candidat 1 | Excellent  | electeur 4 |
	| candidat 1 | Excellent  | electeur 5 |
	| candidat 1 | Passable   | electeur 6 |
	| candidat 2 | Assez bien | electeur 1 |
	| candidat 2 | Correct    | electeur 2 |
	| candidat 2 | Excellent  | electeur 3 |
	| candidat 2 | Passable   | electeur 4 |
	| candidat 2 | Passable   | electeur 5 |
	| candidat 2 | Passable   | electeur 6 |
	When je clôture le "Scrutin 1"
	Then j'obtiens la médiane pour chaque candidat 
		| Option     | Mediane  | Pourcentage inferieur | Pourcentage superieur |
		| candidat 1 | Bien     | 16.67                 | 50                    |
		| candidat 2 | Passable | 0                     | 50                    |
	And le détail
			| Option     | Choix      | NombreDeVote |
			| candidat 1 | A rejeter  | 0            |
			| candidat 1 | Passable   | 1            |
			| candidat 1 | Correct    | 0            |
			| candidat 1 | Assez bien | 0            |
			| candidat 1 | Bien       | 2            |
			| candidat 1 | Très bien  | 1            |
			| candidat 1 | Excellent  | 2            |
			| candidat 2 | A rejeter  | 0            |
			| candidat 2 | Passable   | 3            |
			| candidat 2 | Correct    | 1            |
			| candidat 2 | Assez bien | 1            |
			| candidat 2 | Bien       | 0            |
			| candidat 2 | Très bien  | 0            |
			| candidat 2 | Excellent  | 1            |
	And le nom du vainqueur est 'candidat 1'
	And le resultat est enregistré en base de données 
	| Scrutin   | Nombre votants | Valide | Vainqueur  |
	| Scrutin 1 | 6              | oui    | candidat 1 |

@mytag
Scenario: JugementMajoritaire Identifier candidat vainqueur après égalité mediane
	Given les options suivantes pour le scrutin "Scrutin 1"
	| Nom        |
	| candidat 1 |
	| candidat 2 |
	| candidat 3 |
	And les votes des electeurs pour le "Scrutin 1"
	| Option     | Choix      | Electeur   |
	| candidat 1 | Très bien  | electeur 1 |
	| candidat 1 | Bien       | electeur 2 |
	| candidat 1 | Bien       | electeur 3 |
	| candidat 1 | Excellent  | electeur 4 |
	| candidat 1 | Excellent  | electeur 5 |
	| candidat 1 | Passable   | electeur 6 |
	| candidat 2 | Assez bien | electeur 1 |
	| candidat 2 | Correct    | electeur 2 |
	| candidat 2 | Excellent  | electeur 3 |
	| candidat 2 | Passable   | electeur 4 |
	| candidat 2 | Passable   | electeur 5 |
	| candidat 2 | Passable   | electeur 6 |
	| candidat 3 | Assez bien | electeur 1 |
	| candidat 3 | Excellent  | electeur 2 |
	| candidat 3 | Passable   | electeur 3 |
	| candidat 3 | Bien       | electeur 4 |
	| candidat 3 | Bien       | electeur 5 |
	| candidat 3 | Très bien  | electeur 6 |
	When je clôture le "Scrutin 1"
	Then j'obtiens la médiane pour chaque candidat 
		| Option     | Mediane  | Pourcentage inferieur | Pourcentage superieur |
		| candidat 1 | Bien     | 16.67                 | 50                    |
		| candidat 2 | Passable | 0                     | 50                    |
		| candidat 3 | Bien     | 33.33                 | 33.33                 |
	And le détail
			| Option     | Choix      | NombreDeVote |
			| candidat 1 | A rejeter  | 0            |
			| candidat 1 | Passable   | 1            |
			| candidat 1 | Correct    | 0            |
			| candidat 1 | Assez bien | 0            |
			| candidat 1 | Bien       | 2            |
			| candidat 1 | Très bien  | 1            |
			| candidat 1 | Excellent  | 2            |
			| candidat 2 | A rejeter  | 0            |
			| candidat 2 | Passable   | 3            |
			| candidat 2 | Correct    | 1            |
			| candidat 2 | Assez bien | 1            |
			| candidat 2 | Bien       | 0            |
			| candidat 2 | Très bien  | 0            |
			| candidat 2 | Excellent  | 1            |
			| candidat 3 | A rejeter  | 0            |
			| candidat 3 | Passable   | 1            |
			| candidat 3 | Correct    | 0            |
			| candidat 3 | Assez bien | 1            |
			| candidat 3 | Bien       | 2            |
			| candidat 3 | Très bien  | 1            |
			| candidat 3 | Excellent  | 1            |
	And le nom du vainqueur est 'candidat 1'
	And le resultat est enregistré en base de données
	| Scrutin   | Nombre votants | Valide | Vainqueur  |
	| Scrutin 1 | 6              | oui    | candidat 1 |

@mytag
Scenario: JugementMajoritaire Identifier candidat vainqueur après égalité mediane et pourcentage inf
	Given les options suivantes pour le scrutin "Scrutin 1"
	| Nom        |
	| candidat 1 |
	| candidat 3 |
	And les votes des electeurs pour le "Scrutin 1"
	| Option     | Choix     | Electeur   |
	| candidat 1 | Très bien | electeur 1 |
	| candidat 1 | Bien      | electeur 2 |
	| candidat 1 | Bien      | electeur 3 |
	| candidat 1 | Excellent | electeur 4 |
	| candidat 1 | Excellent | electeur 5 |
	| candidat 1 | Passable  | electeur 6 |
	| candidat 3 | Bien      | electeur 1 |
	| candidat 3 | Excellent | electeur 2 |
	| candidat 3 | Passable  | electeur 3 |
	| candidat 3 | Bien      | electeur 4 |
	| candidat 3 | Bien      | electeur 5 |
	| candidat 3 | Très bien | electeur 6 |
	When je clôture le "Scrutin 1"
	Then j'obtiens la médiane pour chaque candidat 
		| Option     | Mediane  | Pourcentage inferieur | Pourcentage superieur |
		| candidat 1 | Bien     | 16.67                 | 50                    |
		| candidat 3 | Bien     | 16.67                 | 33.33                 |
	And le détail
			| Option     | Choix      | NombreDeVote |
			| candidat 1 | A rejeter  | 0            |
			| candidat 1 | Passable   | 1            |
			| candidat 1 | Correct    | 0            |
			| candidat 1 | Assez bien | 0            |
			| candidat 1 | Bien       | 2            |
			| candidat 1 | Très bien  | 1            |
			| candidat 1 | Excellent  | 2            |
			| candidat 3 | A rejeter  | 0            |
			| candidat 3 | Passable   | 1            |
			| candidat 3 | Correct    | 0            |
			| candidat 3 | Assez bien | 0            |
			| candidat 3 | Bien       | 3            |
			| candidat 3 | Très bien  | 1            |
			| candidat 3 | Excellent  | 1            |
	And le nom du vainqueur est 'candidat 1'
	And le resultat est enregistré en base de données 
	| Scrutin   | Nombre votants | Valide | Vainqueur  |
	| Scrutin 1 | 6              | oui    | candidat 1 |

@mytag
Scenario: JugementMajoritaire pas de candidat vainqueur après égalité parfaite
	Given les options suivantes pour le scrutin "Scrutin 1"
	| Nom        |
	| candidat 1 |
	| candidat 3 |
	And les votes des electeurs pour le "Scrutin 1"
	| Option     | Choix     | Electeur   |
	| candidat 1 | Très bien | electeur 1 |
	| candidat 1 | Bien      | electeur 2 |
	| candidat 1 | Bien      | electeur 3 |
	| candidat 1 | Excellent | electeur 4 |
	| candidat 1 | Excellent | electeur 5 |
	| candidat 1 | Passable  | electeur 6 |
	| candidat 3 | Très bien | electeur 1 |
	| candidat 3 | Bien      | electeur 2 |
	| candidat 3 | Bien      | electeur 3 |
	| candidat 3 | Excellent | electeur 4 |
	| candidat 3 | Excellent | electeur 5 |
	| candidat 3 | Passable  | electeur 6 |
	When je clôture le "Scrutin 1"
	Then j'obtiens la médiane pour chaque candidat 
		| Option     | Mediane | Pourcentage inferieur | Pourcentage superieur |
		| candidat 1 | Bien    | 16.67                 | 50                    |
		| candidat 3 | Bien    | 16.67                 | 50                    |
	And le détail
			| Option     | Choix      | NombreDeVote |
			| candidat 1 | A rejeter  | 0            |
			| candidat 1 | Passable   | 1            |
			| candidat 1 | Correct    | 0            |
			| candidat 1 | Assez bien | 0            |
			| candidat 1 | Bien       | 2            |
			| candidat 1 | Très bien  | 1            |
			| candidat 1 | Excellent  | 2            |
			| candidat 3 | A rejeter  | 0            |
			| candidat 3 | Passable   | 1            |
			| candidat 3 | Correct    | 0            |
			| candidat 3 | Assez bien | 0            |
			| candidat 3 | Bien       | 2            |
			| candidat 3 | Très bien  | 1            |
			| candidat 3 | Excellent  | 2            |
	And on a pas de vainqueur
	And le resultat est enregistré en base de données
	| Scrutin   | Nombre votants | Valide | Vainqueur |
	| Scrutin 1 | 6              | non    | aucun     |

@mytag
Scenario: JugementMajoritaire scrutin déjà cloturé
	Given les options suivantes pour le scrutin "Scrutin 1"
	| Nom        |
	| candidat 1 |
	And les votes des electeurs pour le "Scrutin 1"
	| Option     | Choix     | Electeur   |
	| candidat 1 | Très bien | electeur 1 |
	| candidat 1 | Bien      | electeur 2 |
	| candidat 1 | Bien      | electeur 3 |
	| candidat 1 | Excellent | electeur 4 |
	| candidat 1 | Excellent | electeur 5 |
	| candidat 1 | Passable  | electeur 6 |
	When je clôture le "Scrutin 1" 
	When je clôture le "Scrutin 1" 
	Then on obtiens le message "Ce scrutin est déja cloturé"
