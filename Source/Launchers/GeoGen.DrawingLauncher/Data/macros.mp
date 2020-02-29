u:=5cm;
input TEX;

let old_endfig = endfig;

vardef endfig = 	
	draw bbox(currentpicture) withcolor background;	
	old_endfig
enddef; 

%%%%%%%%%%% TEXT CONVERSION MACROS %%%%%%%%%%%

vardef Table(text t) =

	save s; string s; s := "\tabskip=0pt\halign to \hsize{"
	& "\tabskip0pt plus1fil\hfil#\hfil&"
	& "\tabskip0pt plus1fil\hfil#\hfil&"
	& "\tabskip0pt plus1fil\hfil#\hfil&"
	& "\tabskip0pt\hfil#\hfil\cr "
	& "Type&Ranking&Weight&Contribution\cr "
	& "\noalign{\vskip2pt\hrule\vskip3pt}";
	
	counter := 0;
	
	for i = t:
		counter:=counter+1;	
		s := s & i;
		if (counter mod 4) <> 0:
			s := s & "&";
		else:
			s := s & "\cr ";
		fi
	endfor
		
	s & "}"
	
enddef;

vardef TexTextOnTheRight(expr text) = 
		
	save p; picture p; p:=TEX("\parskip=10pt\parindent=0pt\hsize=" &
	decimal(max(8cm,abs(urcorner bbox(currentpicture) - ulcorner bbox(currentpicture))) ) 
	& "pt\vbox{" & text & "}");
			
	p shifted ((5,0) + urcorner bbox(currentpicture))
	       shifted -(0,ypart urcorner(bbox(p)))
		
enddef;

def Triangle_Text(expr A,B,C) = 
	"Let~$" & A & B & C & "$ be a triangle. "
enddef;

def Midpoint_Text(expr M,A,B) = 
	"Let~$" & M & "$ be the midpoint of~$" & A & B & "$. "		
enddef;

def PointReflection_Text(expr P,A,B) = 
	"Let~$" & P & "$ be the reflection of~$" & A & "$ in~$" & B & "$. "
enddef;

def ParallelLine_Text(expr l,P,k) = 
	"Let~$" & l & "$ be the line through~$" & P & "$ parallel to~$" & k & "$. "		
enddef;

def ParallelLineToLineFromPoints_Text(expr l,A,B,C) = 
	"Let~$" & l & "$ be the line through $" & A & "$ parallel to $" & B & C & "$. "
enddef;

def PerpendicularLineAtPointOfLine_Text(expr l,A,B) = 
	"Let~$" & l & "$ be the line through~$" & A & "$ such that~$" & l & " \perp " & A & B & "$. "		
enddef;

def PerpendicularLineToLineFromPoints_Text(expr l,A,B,C) = 
	"Let~$" & l & "$ be the line through $" & A & "$ perpendicular to $" & B & C & "$. "
enddef;

def PerpendicularBisector_Text(expr l,A,B) = 
	"Let~$" & l & "$ be the perpendicular bisector of the segment~$" & A & B & "$. "
enddef;

def TangentLine_Text(expr l,A,B,C) = 
	"Let~$" & l & "$ be the line tanget to circle~$" & A & B & C & "$ at~$" & A & "$. "
enddef;

def InternalAngleBisector_Text(expr l,A,B,C) = 
	"Let~$" & l & "$ be the internal angle bisector of~$\angle " & B & A & C & "$. "
enddef;

def ExternalAngleBisector_Text(expr l,A,B,C) = 
	"Let~$" & l & "$ be the external angle bisector of~$\angle " & B & A & C & "$. "
enddef;

def PerpendicularProjectionOnLineFromPoints_Text(expr P,A,B,C) = 
	"Let~$" & P & "$ be the projection of~$" & A & "$ on~$" & B & C & "$. "
enddef;

def PerpendicularProjection_Text(expr P,A,l) = 
	"Let~$" & P & "$ be the projection of~$" & A & "$ on~$" & l & "$. "
enddef;

def ReflectionInLineFromPoints_Text(expr P,A,B,C) = 
	"Let~$" & P & "$ be the reflection of~$" & A & "$ in~$" & B & C & "$. "
enddef;

def ReflectionInLine_Text(expr P,l,A) = 
	"Let~$" & P & "$ be the reflection of~$" & A & "$ in~$" & l & "$. "
enddef;

def MidpointOfArc_Text(expr P,A,B,C) = 
	"Let~$" & P & "$ be the midpoint of arc~$" & B & A & C & "$. "
enddef;

def MidpointOfOppositeArc_Text(expr P,A,B,C) = 
	"Let~$" & P & "$ be the midpoint of opposite arc~$" & B & A & C & "$. "
enddef;

def ParallelogramPoint_Text(expr P,A,B,C) = 
	"Let~$" & P & "$ be such a~point that~$" & A & B & P & C & "$ is a~parallelogram. "
enddef;

def OppositePointOnCircumcircle_Text(expr P,A,B,C) = 
	"Let~$" & P & "$ be the point opposite to~$" & A & "$ on circle~$" & B & A & C & "$. "
enddef;

def Circumcenter_Text(expr P,A,B,C) = 
	"Let~$" & P & "$ be the circumcenter of~$" & A & B & C & "$. "
enddef;

def Orthocenter_Text(expr P,A,B,C) = 
	"Let~$" & P & "$ be the orthocenter of~$" & A & B & C & "$. "
enddef;

def Median_Text(expr l,A,B,C) = 
	"Let~$" & l & "$ be the~$" & A & "$-median of~$" & A & B & C & "$. "
enddef;

def IntersectionOfLines_Text(expr P,k,l) = 
	"Let~$" & P & "$ be the intersection point of~$" & k & "$ and $" & l & "$. "		
enddef;

def IntersectionOfLineAndLineFromPoints_Text(expr P,k,A,B) = 
	"Let~$" & P & "$ be the intersection point of~$" & k & "$ and $" & A & B & "$. "		
enddef;

def IntersectionOfLinesFromPoints_Text(expr P,A,B,C,D) = 
	"Let~$" & P & "$ be the intersection point of~$" & A & B & "$ and $" & C & D & "$. "		
enddef;

def Incircle_Text(expr c,A,B,C) = 
	"Let~$" & c & "$ be the incircle of~$" & A & B & C & "$. "		
enddef;

def Excircle_Text(expr c,A,B,C) = 
	"Let~$" & c & "$ be the~$" & A & "$-excircle of~$" & A & B & C & "$. "		
enddef;

def Incenter_Text(expr P,A,B,C) = 
	"Let~$" & P & "$ be the incenter of~$" & A & B & C & "$. "		
enddef;

def Excenter_Text(expr P,A,B,C) = 
	"Let~$" & P & "$ be the~$" & A & "$-excenter of~$" & A & B & C & "$. "		
enddef;

def CircleWithDiameter_Text(expr c,A,B) = 
	"Let~$" & c & "$ be the circle with a~diameter~$" & A & B & "$. "		
enddef;

def EqualLineSegments_Text(expr k,l) =
	"Show that $" & k & "=" & l & "$. "
enddef;

def ConcurrentLines_Text(expr p,q,r) =
	"Show that lines~$" & p & "," & q & "," & r & "$ are concurrent. "
enddef;

def TangentCircles_Text(expr k,l) =
	"Show that circles~$" & k & "$ and $" & l & "$ are tangent to each other. "
enddef;

def LineTangentToCircle_Text(expr k,l) =
	"Show that line~$" & k & "$ and circle $" & l & "$ are tangent to each other. "
enddef;

def ConcyclicPoints_Text(expr A,B,C,D) =
	"Show that points~$" & A & "," & B & "," & C & "," & D & "$ are concyclic. "
enddef;

def CollinearPoints_Text(expr A,B,C) =
	"Show that points~$" & A & "," & B & "," & C & "$ are collinear. "
enddef;

def ParallelLines_Text(expr k,l) =
	"Show that $" & k & " \parallel " & l & "$. "
enddef;

def PerpendicularLines_Text(expr k,l) =
	"Show that $" & k & " \perp " & l & "$. "
enddef;

def Incidence_Text(expr P,k) =
	"Show that $" & P & "$ lies on $" & k & "$. "
enddef;

%%%%%%%%%%% HELPER MACROS %%%%%%%%%%%

%
% Returns a simple point mark, i.e. a circle filled with the background color, scaled by a given coefficient.
%
vardef PointMarkScaled(expr point, scale) = 
	
	% Prepare circle
	save circle; path circle; circle = fullcircle scaled scale shifted point;
	
	% Fill it
	fill circle withcolor background;
	
	% Return it
	circle

enddef;

%
% Returns the circle centered at O with given radius
%
vardef CircleWithRadius(expr O, r) = fullcircle scaled (2r) shifted O enddef;

%
% Labels a given point so that the label is intelligently placed (the code will be cleaned)
%
vardef LabelPoint(expr label, P) =
	
	bboxmargin:=1;
	save pic; picture pic; pic:=currentpicture;
	save box; path box; box:=bbox label;
	
	minD:= 2+abs((llcorner box) + (urcorner box))/2;
	maxD:= 1.5minD;
	
	n:=40;
	m:=20;
	
	vardef place_it(expr angle, d, erasing) = 
		
		save pp; picture pp; 
		bboxmargin:=2;
		
		pp:=label shifted P
		          shifted (d*cosd(angle),d*sind(angle))
				  shifted (-((llcorner box)+(urcorner box))/2);
		
		if erasing:
		
			fill fullcircle scaled (abs(llcorner(bbox pp) - urcorner(bbox pp))/2)
				shifted ((llcorner bbox(pp) + urcorner bbox(pp))/2)
				withcolor background;
			fi;
		
		draw pp;
				
	enddef;
	
	image(	
		save all_c;
		d_index:=0;		
	
		save placed; boolean placed; placed:=false;
	
		for d=minD step ((maxD-minD)/(m-1)) until maxD:
			
			for k=0 upto n-1:
						
				save new_box; path new_box;
				
				new_box:=bbox(label shifted (P+(d*cosd(k*(360/n)),d*sind(k*(360/n)))
								-((llcorner box)+(urcorner box))/2));
				
				c:=0;
														
				for item within pic:
					
					save p,q,r,z; path p,q,r; pair z[];
		
					p:= new_box;
					q:= pathpart item;					
					r:= p;
					
					i:= 0;
					
					forever:
					
						save a;	
						(a, whatever) = q intersectiontimes r;
						
						if a <>-1:
							zz[i]:=a;
						fi;
					
						r := r cutbefore q;
						exitif length cuttings = 0;
						
						
						factor:=epsilon;
						
						forever: 
							z[i]:= (point 0 of r);
							r := subpath(factor, length r) of r;	
							factor:= 2factor;
							exitif i=0;
							exitif abs(z[i-1]-z[i]) > eps;
						endfor;
						
						i := i + 1;
					endfor;
					
					vardef is_inside(expr p) = 
					    ((xpart p >= xpart (ulcorner new_box)) and
						( xpart p <= xpart (urcorner new_box)) and
						( ypart p >= ypart (llcorner new_box)) and
						( ypart p <= ypart (ulcorner new_box)))
					enddef;
					
					if i=0:
						if is_inside(point (length q)/2 of q):
							c:= c + abs((point 0 of q) - (point length q of q));
						fi;
					
					elseif i=1:
						
						% We need to find out which endpoint of the path
						% is inside. 
						save ends; pair ends[];
						
						% Get them first
						ends[0] = point 0 of q;
						ends[1] = point infinity of q;
						
										
						if (is_inside(ends[0])):
							c:= c + abs(ends[0]-z[0]);
						else:                       
							c:= c + abs(ends[1]-z[0]);
						fi;	                        
						                            
					else:						    
						for j=0 upto (i-2):         
							c:= c + abs(z[j]-z[j+1]);
						endfor;						
					fi;		
				
				endfor;
						
				all_c[d_index][k]:=c;
						
				if all_c[d_index][k] < 0:
					all_c[d_index][k]:=0;
				fi;
									
			endfor;
						
			save smallest; smallest:=infinity; 
			save last_non_zero_index; last_non_zero_index:=-1;
			
			for i=0 upto n-1:
			
				if all_c[d_index][i] < smallest:
					smallest:=all_c[d_index][i];
				fi;
				
				if all_c[d_index][i] > 0:
					last_non_zero_index:=i;
				fi;
				
			endfor;
						
			if last_non_zero_index=-1:
				place_it(90,d,false);
				placed:=true;
				exitif true;
			fi;
			
			if smallest=0:
						
				i:=last_non_zero_index;
				
				forever:
					
					exitif all_c[d_index][i] = 0;
					
					i:=i+1;
					if i=n:
						i:=0;
					fi;					
					
				endfor;
								
				longest:=0;
				longest_start:=0;
				longest_end:=0;
				
				save in; boolean in; in:=false;
				current_length := 0;
				current_start := 0;
				current_end := 0;
				
				j:=i;
				
				forever:
								
					if all_c[d_index][j]=0:
						
						if in:
							current_length:=current_length+1;
							current_end:=j;
						else:
							in:=true;
							current_start:=j;
							current_end:=j;
							current_length:=1;
						fi;
					else:
						
						if in:
							in:=false;
							
							if current_length > longest:
								longest:= current_length;
								longest_start:= current_start;
								longest_end:= current_end;
							fi;
						fi;
					fi;
						
					
					j:=j+1;
					if j=n:
						j:=0;
					fi;
					
					exitif j=i;
				
				endfor;
								
				if longest_start > longest_end:
					longest_end:=longest_end + n;
				fi;
				
				place_it((360/n)*(longest_start+longest_end)/2,d,false);
				placed:=true;				
				exitif true;				
			fi;
			
			d_index:=d_index+1;
		
		endfor;
		
		if not placed:
			
			minimal:=infinity;
			min_d:=-1;
			min_k:=-1;
			
			for d=0 upto (d_index-1):
				for k=0 upto n-1:
					if all_c[d][k] < minimal:
						min_d:=d;
						min_k:=k;
						minimal:=all_c[d][k];
					fi;
				endfor;
			endfor;
			
			place_it((360/n)*min_k,minD+min_d*(maxD-minD)/(m-1),true);
						
		fi;
	)
	
enddef;

%%%%%%%%%%% LINE STYLES %%%%%%%%%%%

def WithAuxiliaryStyle = 
	withpen pencircle scaled 0.5
	dashed withdots scaled 0.3
enddef;


def WithNormalStyle =
	withpen pencircle scaled 0.5
enddef;


def WithTheoremStyle = 
	withpen pencircle scaled 0.5
	withcolor red
enddef;

%%%%%%%%%%% DRAWING MACROS %%%%%%%%%%%


def CircleAuxiliaryStyle(expr center, radius) = 
	CircleWithRadius(center, radius) WithAuxiliaryStyle
enddef;


def CircleNormalStyle(expr O, r) = 
	CircleWithRadius(O, r) WithNormalStyle
enddef;


def CircleTheoremStyle(expr O, r) = 
	CircleWithRadius(O, r) WithTheoremStyle
enddef;


def LineSegmentAuxiliaryStyle(expr A, B) = 
	A--B WithAuxiliaryStyle
enddef;


def LineSegmentNormalStyle(expr A, B) = 
	A--B 
enddef;


def LineSegmentTheoremStyle(expr A, B) = 
	A--B WithTheoremStyle
enddef;


def PointMarkAuxiliaryStyle(expr P) = 
	PointMarkScaled(P, 1.5)
enddef;


def PointMarkNormalStyle(expr P) = 
	PointMarkScaled(P, 2.4)
enddef;


def PointMarkTheoremStyle(expr P) = 
	PointMarkScaled(P, 2.4) withcolor red
enddef; 